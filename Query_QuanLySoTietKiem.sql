use master
go
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
passbook_balance bigint not null,
passbook_type int not null,
passbook_customer int not null,
opendate datetime default getdate(),
withdrawday datetime default getdate(),
status int default 1
)

create table typepassbook
(
id int identity(1,1) primary key,
typename nvarchar(200) default N'Không kì hạn',
interest_rate float not null,
term int default 0,
kind nvarchar(200) default N'Không kì hạn',
withdrawterm int default 0,
min_collectmoney bigint default 0,
min_passbookbalance bigint default 0
)
create table collectbill
(
id nvarchar(20) primary key,
collect_passbook int not null,
collectmoney bigint not null,
collectdate datetime default getdate(),
)
create table withdrawbill
(
id nvarchar(20) primary key,
withdraw_passbook int not null,
withdrawmoney bigint not null	,
withdrawdate datetime default getdate()
)
alter table passbook add constraint fk_customer_id foreign key(passbook_customer) references customer(id)
alter table passbook add constraint fk_typepassbook_id foreign key(passbook_type) references typepassbook(id)
alter table collectbill add constraint fk_passbook_id_1 foreign key (collect_passbook) references passbook(id)
alter table withdrawbill add constraint fk_passbook_id_2 foreign key (withdraw_passbook) references passbook(id)
alter table passbook add constraint ckeck_status check (status in(0,1))
go

create trigger TG_Checkpassbookbalanace on passbook
for insert
as
begin
declare @balance money;
select @balance= (select passbook_balance from inserted);
if(@balance <(select min_passbookbalance from typepassbook, inserted where inserted.passbook_type=typepassbook.id))
begin 
print N'Số tiền gởi ban đầu không hợp lệ'
rollback tran
end
else 
print N'Tạo sổ tiết kiệm thành công'
end

go
create trigger trg_ckeckcollectmoney on collectbill
for insert
as
begin
declare @money money
declare @date datetime
declare @id int
declare @collectday datetime
select @id=collect_passbook from inserted
select @date=collectdate from inserted
select @collectday=withdrawday from passbook where id=@id 
if(day(@date)=day(@collectday) and month(@date)=month(@collectday) and year(@date)=year(@collectday))
begin
select @money=(select collectmoney from inserted)
if(@money<(select min_collectmoney from typepassbook,inserted,passbook where inserted.collect_passbook=passbook.id and passbook.passbook_type=typepassbook.id))
begin
print N'Số tiền gởi thêm không hợp lệ'
rollback tran
end
else
begin
print N'Tạo phiếu gởi thành công'
update passbook set passbook_balance=passbook_balance+ (select collectmoney from inserted) where id=@id
end
end
else
begin 
print N'Chưa đến ngày đáo hạn sổ tiết kiệm'
rollback tran
end
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
set @id='W'+cast(@max as nvarchar(18))
end
update withdrawbill set id=@id where @iID=withdrawbill.id
end
go
create trigger insert_typepasbook on typepassbook
for insert,update
as
begin
declare @term int
declare @id int
select @id=id from inserted
select @term= term from inserted
declare @name nvarchar(200)
set @name=N'Kì hạn '+cast(@term as nvarchar(196))+N' tháng'
declare @count int
select @count=(select count(*) from typepassbook where @name=typename)
if(@count>0)
begin
print N'Trùng tên loại tiết kiệm'
rollback tran
end
if(@term > 0)
begin
update typepassbook set kind = N'Có kì hạn' where typepassbook.id=@id;
update typepassbook set typename=@name where typepassbook.id=@id;
update typepassbook set withdrawterm=@term*30 where typepassbook.id=@id
end
else
update typepassbook set withdrawterm=15 where typepassbook.id=@id
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
create trigger insert_withdrawbill on withdrawbill
for insert
as
begin
declare @id int
declare @money bigint
declare @day datetime
select @id=withdraw_passbook from inserted
select @money=withdrawmoney from inserted
declare @kind nvarchar(200) 
select @kind=kind from typepassbook, passbook where @id=passbook.id and passbook_type= typepassbook.id

if(@kind=N'Có kì hạn')
begin
if(@money!=(select passbook_balance from passbook where @id=id))
begin
print N'Loại tiết kiệm có kỳ hạn phải rút hết'
rollback tran
end
else
begin
print N'Hoàn tất giao dịch, sổ bị xóa'
update passbook set status=0 where id=@id 
end
end
update passbook set passbook_balance=passbook_balance-(select withdrawmoney from inserted) where id=@id
end
go
create function reportC
(@id int,
@day int,@month int, @year int)
returns bigint
as
begin 
declare @value bigint
select @value =(select sum(collectmoney) 'collectmoney1'
from dbo.collectbill, dbo.typepassbook, dbo.passbook
where day(collectdate)=@day and month(collectdate)=@month and year(collectdate)=@year and passbook.id= collect_passbook and passbook_type = typepassbook.id group by typepassbook.id having typepassbook.id=@id) 
return @value
end
go
create function reportW
(@id int,
@day int,@month int, @year int)
returns bigint
as
begin
declare @value1 bigint
select @value1 = (select sum(withdrawmoney)
from dbo.withdrawbill, dbo.typepassbook, dbo.passbook 
where day(withdrawdate)=@day and month(withdrawdate)=@month and year(withdrawdate)=@year and passbook.id= withdraw_passbook and passbook_type = typepassbook.id group by typepassbook.id having typepassbook.id=@id)
return @value1
end
go
create function reportCM
(@id int,
@month int, @year int)
returns bigint
as
begin 
declare @value bigint
select @value =(select sum(collectmoney) 'collectmoney1'
from dbo.collectbill, dbo.typepassbook, dbo.passbook
where month(collectdate)=@month and year(collectdate)=@year and passbook.id= collect_passbook and passbook_type = typepassbook.id group by typepassbook.id having typepassbook.id=@id) 
return @value
end
go
create function reportWM
(@id int,
@month int, @year int)
returns bigint
as
begin
declare @value1 bigint
select @value1 = (select sum(withdrawmoney)
from dbo.withdrawbill, dbo.typepassbook, dbo.passbook 
where month(withdrawdate)=@month and year(withdrawdate)=@year and passbook.id= withdraw_passbook and passbook_type = typepassbook.id group by typepassbook.id having typepassbook.id=@id)
return @value1
end
go
create proc usp_ReportTypePassbookDay
@day int,
@month int,
@year int
as
begin
select ROW_NUMBER() over(order by typename) STT , typename TypePassbook, collect MoneyIncome , withdraw MoneyOutcome, abs(collect - withdraw) Difference
from (select id as idc,
case when collect is null then 0 else collect end as collect,
case when withdraw is null then 0 else withdraw end as withdraw
from 
 (select id, dbo.reportC(id,@day,@month,@year) as collect , dbo.reportW(id,@day,@month,@year) as withdraw
 from typepassbook)as a) b,typepassbook where idc=typepassbook.id
end
go
create proc usp_ReportTypePassbookMonth
@month int,
@year int,
@typeid int
as
begin
select ROW_NUMBER() over(order by typename) STT , typename TypePassbook, collect MoneyIncome , withdraw MoneyOutcome, abs(collect - withdraw) Difference
from (select id as idc,
case when collect is null then 0 else collect end as collect,
case when withdraw is null then 0 else withdraw end as withdraw
from 
 (select id, dbo.reportCM(id,@month,@year) as collect , dbo.reportWM(id,@month,@year) as withdraw
 from typepassbook)as a) b,typepassbook where idc=typepassbook.id and typepassbook.id=@typeid
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
create proc usp_InsertPassbook
@type int,
@balance money,
@cusid int,
@day datetime
as
begin
insert dbo.passbook
(passbook_type,passbook_balance,passbook_customer,opendate)
values
(@type,
@balance,
@cusid,
@day)
end
go
create proc usp_InsertPassbook1
@type int,
@balance money,
@cusid int
as
begin
insert dbo.passbook
(passbook_type,passbook_balance,passbook_customer)
values
(@type,
@balance,
@cusid)
end
go
create proc usp_InsertCustomer 
@cmnd char(9),
@name nvarchar(200),
@address nvarchar(200)
as
begin
insert dbo.customer(cmnd,cus_name,cus_address)
values( @cmnd,
@name,
@address)
end
go
create proc usp_InsertTypePassbook
@interset_rate float,
@term int,
@min_balance money,
@min_collectmoney money
as
begin
insert dbo.typepassbook(
interest_rate,
term,
min_passbookbalance,
min_collectmoney)
values(@interset_rate,
@term,
@min_balance,
@min_collectmoney)
end
go
create proc usp_Insertcollectbill
@id int,
@passbook int,
@money money,
@day datetime
as
begin
insert dbo.collectbill(id,collect_passbook,collectmoney,collectdate)
values(@id,
@passbook,
@money,
@day)
end

go
create proc usp_Insertwithdrawbill
@id int,
@passbook int,
@money money,
@day datetime
as
insert dbo.withdrawbill(id,withdraw_passbook,withdrawmoney,withdrawdate)
values
(@id,
@passbook,
@money,
@day)
go
-- DU LIEU GIA
--typepassbook
exec usp_InsertTypePassbook 0.5,3,1000000,100000
exec usp_InsertTypePassbook 0.55,6,1000000,100000
exec usp_InsertTypePassbook 0.15,0,1000000,100000
select * from dbo.typepassbook
-- customer
exec usp_InsertCustomer 8123,N'Trần Hiệp Nguyên Huy',N'Đồng Tháp'
exec usp_InsertCustomer 1234,N'Nguyễn Văn A',N'Đồng Tháp'
exec usp_InsertCustomer 1235,N'Nguyễn Văn B',N'Hồ Chí Minh'
exec usp_InsertCustomer 1236,N'Nguyễn Văn C',N'Vĩnh Long'
exec usp_InsertCustomer 1237,N'Nguyễn Văn D',N'Tây Ninh'
exec usp_InsertCustomer 1238,N'Nguyễn Văn E',N'Trà Vinh'
exec usp_InsertCustomer 1239,N'Nguyễn Văn F',N'Đồng Tháp'
exec usp_InsertCustomer 1213,N'Nguyễn Văn G',N'Vĩnh Long'
exec usp_InsertCustomer 1223,N'Nguyễn Văn H',N'Vĩnh Long'
exec usp_InsertCustomer 1233,N'Nguyễn Văn I',N'Đồng Tháp'
exec usp_InsertCustomer 1243,N'Nguyễn Văn K',N'Trà Vinh'
exec usp_InsertCustomer 1253,N'Nguyễn Văn L',N'Hồ Chí Minh'
exec usp_InsertCustomer 1263,N'Trần Văn A',N'Trà Vinh'
exec usp_InsertCustomer 1273,N'Trần Văn B',N'Đồng Tháp'
exec usp_InsertCustomer 1283,N'Trần Văn C',N'Trà Vinh'
exec usp_InsertCustomer 1293,N'Trần Văn D',N'Hồ Chí Minh'
exec usp_InsertCustomer 1123,N'Trần Văn E',N'Trà Vinh'
exec usp_InsertCustomer 4223,N'Trần Văn F',N'Trà Vinh'
exec usp_InsertCustomer 1323,N'Trần Văn G',N'Hồ Chí Minh'
exec usp_InsertCustomer 1423,N'Trần Văn H',N'Hồ Chí Minh'
select * from customer
-- passbook
exec usp_InsertPassbook 1,2000000,1,'20190215'
exec usp_InsertPassbook 2,1000000,2,'20170601'
exec usp_InsertPassbook 3,3000000,3,'20190722'
exec usp_InsertPassbook 1,6000000,4,'20180813'
exec usp_InsertPassbook 2,8000000,5,'20180912'
exec usp_InsertPassbook 3,10000000,6,'20190405'
exec usp_InsertPassbook 1,2000000,7,'20191104'
exec usp_InsertPassbook 2,2000000,8,'20191228'
exec usp_InsertPassbook 3,5000000,9,'20190120'
exec usp_InsertPassbook 1,2000000,10,'20180228'
exec usp_InsertPassbook 2,1000000,11,'20190330'
exec usp_InsertPassbook 3,1000000,12,'20180430'
exec usp_InsertPassbook 1,1000000,13,'20170531'
exec usp_InsertPassbook 2,2000000,14,'20180616'
exec usp_InsertPassbook 3,9000000,15,'20180717'
exec usp_InsertPassbook 1,6000000,16,'20170818'
exec usp_InsertPassbook 2,4000000,17,'20180922'
exec usp_InsertPassbook 3,8000000,18,'20191023'
exec usp_InsertPassbook 1,7000000,19,'20190521'
exec usp_InsertPassbook 1,2000000,20,'20170426'
select * from dbo.passbook
--collectbill.
exec usp_Insertcollectbill 1,1,200000,'20190516'
exec usp_Insertcollectbill 2,2,500000,'20171128'
exec usp_Insertcollectbill 3,3,200000,'20190806'
exec usp_Insertcollectbill 4,4,800000,'20181111'
exec usp_Insertcollectbill 5,5,200000,'20190311'
exec usp_Insertcollectbill 5,6,200000,'20190420'
exec usp_Insertcollectbill 6,7,200000,'20200202'
exec usp_Insertcollectbill 7,8,10000000,'20200625'
exec usp_Insertcollectbill 8,9,2000000,'20190204'
exec usp_Insertcollectbill 9,10,400000,'20180529'
exec usp_Insertcollectbill 10,11,300000,'20190926'
select * from collectbill
select* from passbook
--withdrawbill
exec usp_Insertwithdrawbill 1,12,200000,'20180515'
exec usp_Insertwithdrawbill 2,13,1000000,'20170829'
exec usp_Insertwithdrawbill 3,14,2000000,'20181213'
exec usp_Insertwithdrawbill 4,15,800000,'20180801'
exec usp_Insertwithdrawbill 5,16,6000000,'20171116'
exec usp_Insertwithdrawbill 6,17,4000000,'20190321'
exec usp_Insertwithdrawbill 7,18,7000000,'20191107'
exec usp_Insertwithdrawbill 8,19,7000000,'20190819'
exec usp_Insertwithdrawbill 9,20,2000000,'20170725'
select * from passbook
select * from withdrawbill
go




