drop database savingpassbook
go
create database savingpassbook
go
use savingpassbook
go	
create table customer
(
id int identity(1,1) primary key,
cus_name nvarchar(200) not null,
cus_address nvarchar(200)default null,
cmnd char(9) not null unique
)

create table passbook
(
id int identity(1,1) primary key,
passbook_balance money not null,
passbook_type int not null,
passbook_customer int not null,
opendate datetime default getdate(),
withdrawday datetime default getdate(),
)

create table typepassbook
(
id int identity(1,1) primary key,
typename nvarchar(200) default 'Không kì hạn',
interest_rate float not null,
term int default 0,
kind nvarchar(200) default 'Không kì hạn',
withdrawterm int default 0,
min_collectmoney money default 0,
min_passbookbalance money default 0
)
create table collectbill
(
id nvarchar(20) primary key,
collect_passbook int not null,
collectmoney  money not null,
collectdate datetime default getdate(),
)
create table withdrawbill
(
id nvarchar(20) primary key,
withdraw_passbook int not null,
withdrawmoney money not null	,
withdrawdate datetime default getdate()
)
alter table passbook add constraint fk_customer_id foreign key(passbook_customer) references customer(id)
alter table passbook add constraint fk_typepassbook_id foreign key(passbook_type) references typepassbook(id)
alter table collectbill add constraint fk_passbook_id_1 foreign key (collect_passbook) references passbook(id)
alter table withdrawbill add constraint fk_passbook_id_2 foreign key (withdraw_passbook) references passbook(id)

go

create trigger TG_Checkpassbookbalanace on passbook
for insert
as
begin
declare @balance money;
select @balance= (select passbook_balance from inserted);
if(@balance <(select min_passbookbalance from typepassbook, inserted where inserted.passbook_type=typepassbook.id))
begin 
print 'Số tiền gởi ban đầu không hợp lệ'
rollback tran
end
else 
print 'Tạo sổ tiết kiệm thành công'
end

go
create trigger trg_ckeckcollectmoney on collectbill
for insert
as
begin
declare @money money 
select @money=(select collectmoney from inserted)
if(@money<(select min_collectmoney from typepassbook,inserted,passbook where inserted.collect_passbook=passbook.id and passbook.passbook_type=typepassbook.id))
begin
print'Số tiền gởi thêm không hợp lệ'
rollback tran
end
else 
print'Tạo phiếu gởi thành công'
end
 
go
create trigger trg_change_id_collectbill on collectbill
for insert
as
begin
declare @max int
declare @iID nvarchar(20)
declare @id nvarchar(20)
select @max=(select count(*) from collectbill )
select @iID=id from inserted
select @id='C'+cast(@max as nvarchar(18))
while(exists (select id from collectbill where @id=id))
begin
set @max=@max+1
set @id='C'+cast(@max as nvarchar(18))
end
update collectbill set id=@id where @iID=collectbill.id
end
go
create trigger trg_changeid_withdrawtbill on withdrawbill
for insert
as
begin
declare @max int
declare @iID nvarchar(20)
declare @id nvarchar(20)
select @max=(select count(*) from withdrawbill )
select @iID=id from inserted
select @id='W'+cast(@max as nvarchar(18))
while(exists (select id from withdrawbill where @id=id))
begin
set @max=@max+1
set @id='C'+cast(@max as nvarchar(18))
end
update withdrawbill set id=@id where @iID=withdrawbill.id
end
go
create trigger insert_typename on typepassbook
for insert,update
as
begin
declare @term int
declare @id int
select @id=id from inserted
select @term= term from inserted
declare @name nvarchar(200)
set @name='Kì hạn'+cast(@term as nvarchar(196))+'tháng'
if(exists(select typename from typepassbook where @name=typename))
begin
print'Trung ten loai tiet kiem'
rollback tran
end
if(@term > 0)
begin
update typepassbook set kind ='Có kì hạn' where typepassbook.id=@id;
update typepassbook set typename=@name where typepassbook.id=@id;
end
end

go
create trigger check_collectbill on collectbill
for insert
as
begin

end
go
create trigger insert_withdrawday on passbook
for insert
as
begin
declare @withdrawterm int
select @withdrawterm=withdrawterm from typepassbook, inserted where(select passbook_type from inserted)=typepassbook.id
update passbook set withdrawday=dateadd(day,@withdrawterm,opendate) where id=(select id from inserted)
end
go
create function find_date
( @id int)
returns datetime
as
 begin
 declare @ngay datetime
select @ngay=(select top(1) ngay  from 
(select withdraw_passbook as di, withdrawdate as ngay from withdrawbill where withdraw_passbook in(select passbook.id from passbook, customer where passbook.passbook_customer= customer.id and customer.id=@id)
union
select collect_passbook, collectdate  from collectbill where collect_passbook in(select passbook.id from passbook, customer where passbook.passbook_customer= customer.id and customer.id =@id)
union
select id,opendate from passbook where id in(select passbook.id from passbook, customer where passbook.passbook_customer= customer.id and customer.id=@id)) as d, customer, passbook
where di=passbook.id and passbook_customer=customer.id 
order by ngay desc)
return @ngay
end
go
create proc usp_ReportTypePassbookDay
@day int,
@month int,
@year int
as
begin
select ROW_NUMBER() over(order by typename) STT , typename TypePassbook, collect MoneyIncome , withdraw MoneyOutcome, abs(collect - withdraw) Difference 
from(select typepassbook.id as idc,
case when collectmoney1 is null then 0 else collectmoney1 end as collect,
case when withdrawmoney1 is null then 0 else withdrawmoney1 end as 'withdraw' 
from (select id1, collectmoney1, withdrawmoney1 
from (select typepassbook.id as 'id1', sum(collectmoney) 'collectmoney1'
from dbo.collectbill, dbo.typepassbook, dbo.passbook
where day(collectdate)=@day and month(collectdate)=@month and year(collectdate)=@year and passbook.id= collect_passbook and passbook_type = typepassbook.id group by typepassbook.id) 
as a full join
(select typepassbook.id 'id2', sum(withdrawmoney) 'withdrawmoney1' 
from dbo.withdrawbill, dbo.typepassbook, dbo.passbook 
where day(withdrawdate)=@day and month(withdrawdate)=@month and year(withdrawdate)=@year and passbook.id= withdraw_passbook and passbook_type = typepassbook.id group by typepassbook.id) as b on(id1= id2)) c full join dbo.typepassbook on (typepassbook.id= id1)) as d, dbo.typepassbook where idc=typepassbook.id
end
go
create proc usp_ReportTypePassbookMonth
@month int,
@year int,
@typeid int
as
begin
select ROW_NUMBER() over(order by typename) STT , typename TypePassbook , collect MoneyIncome , withdraw MoneyOutcome, abs(collect-withdraw) Difference 
from (select typepassbook.id as idc,
case when collectmoney1 is null then 0 else collectmoney1 end as collect,
case when withdrawmoney1 is null then 0 else withdrawmoney1 end as withdraw 
from (select id1, collectmoney1, withdrawmoney1 from (select typepassbook.id as 'id1',sum(collectmoney) 'collectmoney1' 
from dbo.collectbill, dbo.typepassbook, dbo.passbook 
where month(collectdate)=@month and year(collectdate)=@year and passbook.id=collect_passbook and passbook_type=typepassbook.id group by typepassbook.id) 
as a full join 
(select typepassbook.id 'id2',sum(withdrawmoney) 'withdrawmoney1' 
from dbo.withdrawbill, dbo.typepassbook, dbo.passbook 
where month(withdrawdate)=@month and year(withdrawdate)=@year and passbook.id=withdraw_passbook and passbook_type=typepassbook.id group by typepassbook.id) as b on (id1=id2)) c full join dbo.typepassbook on (typepassbook.id=id1)) as d, dbo.typepassbook 
where idc=typepassbook.id and typepassbook.id=@typeid
end
go
