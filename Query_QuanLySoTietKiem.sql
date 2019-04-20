drop database savingpassbook
create database savingpassbook
go
use savingpassbook
go
create table customer
(
id nvarchar(20) primary key,
cus_name nvarchar(200) not null,
cus_address nvarchar(200),
cmnd char(9) not null
)

create table passbook
(
id int identity(1,1) primary key,
passbook_balance money not null,
passbook_type int not null,
passbook_customer nvarchar(20) not null,
opendate datetime default getdate()
)

create table typepassbook
(
id nvarchar(20) primary key,
typename nvarchar(200) not null,
interest_rate float not null,
term int default 0,
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
create table parameter
(
atleast_collectmoney money default 0,
atleast_passbookbalance money default 0	
)
alter table passbook add constraint fk_customer_id foreign key(passbook_customer) references customer(id)
alter table passbook add constraint fk_typepassbook_id foreign key(passbook_type) references typeaccount(id)
alter table collectbill add constraint fk_passbook_id foreign key (collect_passbook) references passbook(id)
alter table withdrawbill add constraint fk_passbook_id foreign key (withdraw_passbookt) references passbook(id)

