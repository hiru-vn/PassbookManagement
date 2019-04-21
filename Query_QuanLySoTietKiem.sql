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
id int primary key identity(1,1),
cus_name nvarchar(200) not null,
cus_address nvarchar(200),
cmnd char(9) not null
)
go
create table passbook
(
id int identity(1,1) primary key,
passbook_balance money not null,
passbook_type int not null,
passbook_customer int not null,
opendate datetime default getdate()
)
go
create table typepassbook
(
id int primary key,
typename nvarchar(200) not null,
interest_rate float not null,
term int default 0,
)
go
create table collectbill
(
id nvarchar(20) primary key,
collect_passbook int not null,
collectmoney  money not null,
collectdate datetime default getdate(),
)
go
create table withdrawbill
(
id nvarchar(20) primary key,
withdraw_passbook int not null,
wihtdrawmoney money not null	,
withdrawdate datetime default getdate()
)
go
create table parameter
(
atleast_collectmoney money default 0,
atleast_passbookbalance money default 0	
)
go
alter table passbook add constraint fk_customer_id foreign key(passbook_customer) references customer(id)
go
alter table passbook add constraint fk_typepassbook_id foreign key(passbook_type) references typepassbook(id)
go
alter table collectbill add constraint fk_passbook_id_1 foreign key (collect_passbook) references passbook(id)
go
alter table withdrawbill add constraint fk_passbook_id_2 foreign key (withdraw_passbook) references passbook(id)
go


