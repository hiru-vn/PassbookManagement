drop database savingpassbook
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
opendate datetime default getdate()
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
wihtdrawmoney money not null	,
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
for insert
as
begin
declare @term int
declare @id int
select @id=id from inserted
select @term= term from inserted
if(@term > 0)
begin
update typepassbook set typename='Kì hạn'+cast(@term as nvarchar(196))+'tháng' where typepassbook.id=@id;
end
end
go
create trigger insert_kind on typepassbook
for insert
as
begin
declare @term int
declare @id int
select @id=id from inserted
select @term= term from inserted
if(@term > 0)
begin
update typepassbook set kind ='Có kì hạn' where typepassbook.id=@id;
end
end