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
	status int default 0
	)

	create table typepassbook
	(
	id int identity(1,1) primary key,
	typename nvarchar(200) default N'Không kì hạn',
	interest_rate float not null,
	term int default 0 unique,
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
	--Trigger kiểm tra số dư ban đầu
	create trigger TG_Checkpassbookbalanace on passbook
	for update
	as
	begin
	declare @balance money;
	select @balance= (select passbook_balance from inserted);
	declare @status int
	select @status=status from inserted
	declare @id int
	select @id=id from inserted
	if(((select count(*) from withdrawbill where withdraw_passbook=@id)=0))
	begin
	if(@balance!=0)
	begin
	if(@balance <(select min_passbookbalance from typepassbook, inserted where inserted.passbook_type=typepassbook.id) and @status=0)
	begin 
	print N'Số tiền gởi ban đầu không hợp lệ'
	rollback tran
	end
	else 
	begin
	update passbook set status=1 where id=@id
	end
	end
	end
	end
	go
	--Triger kiểm tra việc tạo phiếu gởi 
	create trigger trg_insertcollectbill on collectbill
	for insert
	as
	begin
	declare @money money
	declare @date datetime
	declare @id int
	declare @collectday datetime
	declare @balance bigint
	declare @status int
	select @id=collect_passbook from inserted
	select @date=collectdate from inserted
	select @balance=passbook_balance from passbook where id=@id
	select @status=status from passbook where id=@id
	select @collectday=withdrawday from passbook where id=@id 
	select @money=(select collectmoney from inserted)
	if(@balance=0 and @status=0)
	begin
	update passbook set passbook_balance=passbook_balance+ (select collectmoney from inserted) where id=@id
	end
	else
	begin
	if(day(@date)=day(@collectday) and month(@date)=month(@collectday) and year(@date)=year(@collectday))
	begin
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
	end
	go
	--trigger đổi id collect
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
	--Trigger đổi id ưithdraw
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
	--Trigger tự tạo cái thuộc tính của loại tiết kiệm
	create trigger insert_typepasbook on typepassbook
	for insert
	as
	begin
	declare @term int
	declare @id int
	select @id=id from inserted
	select @term= term from inserted
	declare @name nvarchar(200)
	if(@term > 0)
		set @name=N'Kì hạn '+cast(@term as nvarchar(196))+N' tháng'
	else
		set @name=N'Không kì hạn'
	declare @count int
	select @count=(select count(*) from typepassbook where typename=@name)
	if(@count>0)
	begin
	print N'Trùng tên loại tiết kiệm'
	rollback
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
	--trigger update 
	create trigger update_typepasbook on typepassbook
	for update
	as
	begin
	declare @term int
	declare @id int
	select @id=id from inserted
	select @term= term from inserted
	declare @name nvarchar(200)
	set @name=N'Kì hạn '+cast(@term as nvarchar(196))+N' tháng'
	declare @count int
	select @count=(select count(*) from typepassbook where typename=@name)
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
	--Trigger set withdrawday cho passbook
	create trigger insert_withdrawday on passbook
	for insert
	as
	begin
	declare @withdrawterm int
	select @withdrawterm=withdrawterm from typepassbook, inserted where(select passbook_type from inserted)=typepassbook.id
	update passbook set withdrawday=dateadd(day,@withdrawterm,opendate) where id=(select id from inserted)
	end
	go
	--Trigger kiểm tra withdrawbill 
	create trigger insert_withdrawbill on withdrawbill
	for insert
	as
	begin
	declare @id int
	declare @money bigint
	declare @date datetime
	declare @balance bigint
	select @id=withdraw_passbook from inserted
	select @money=withdrawmoney from inserted
	select @date=withdrawdate from inserted
	select @balance=passbook_balance from passbook where id=@id
	declare @kind nvarchar(200) 
	select @kind=kind from typepassbook, passbook where @id=passbook.id and passbook_type= typepassbook.id
	declare @withdrawdate datetime
	select @withdrawdate = withdrawday from passbook where id=@id
	if (@date>@withdrawdate)
	begin
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
	update passbook set passbook_balance=passbook_balance-(select withdrawmoney from inserted) where id=@id
	end
	end
	else
	begin
	if(@money<=@balance)
	update passbook set passbook_balance=passbook_balance-(select withdrawmoney from inserted) where id=@id
	else
	begin
	print 'Số tiền rút vượt quá số dư'
	rollback tran
	end
	end
	end
	else
	begin
	print N'Chưa tới ngày rút tiền'
	rollback tran
	end
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
	--Tạo report
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
	--Upadate Khách hàng bằng id
	create proc usp_Update_cus 
	@id int,
	@name nvarchar(200),
	@address nvarchar(200),
	@cmnd char(9)
	as
	begin
	update customer set cus_name='N''+  @name + ''', cus_address= 'N'' + @address + ''', cmnd=@cmnd where id=@id 
	end
	go
	--Tạo Passbook có ngày tạo sổ 
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
	--Tạo Passbook không có ngày tạo sổ
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
	--Tạo khách hàng
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
	--Tạo loại tiết kiệm
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
	--Tạo phiếu gởi tiền có ngày tạo phiếu
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
	--Tạo phiếu gởi không có ngày tạo phiếu
	create proc usp_Insertcollectbill1
	@id int,
	@passbook int,
	@money money
	as
	begin
	insert dbo.collectbill(id,collect_passbook,collectmoney)
	values(@id,
	@passbook,
	@money)
	end
go
create function reportTypeOpen
(@id int,
@day int,
@month int, @year int)
returns int
as
begin
declare @value int
select @value = (select count(*) from dbo.typepassbook as t,dbo.passbook as p where p.passbook_type = t.id and  t.id = @id and  month(p.opendate) = @month and year(p.opendate) = @year and day(p.opendate) = @day)
return @value
end
go
create function reportTypeClose
(@id int,
@day int,
@month int, @year int)
returns int
as
begin
declare @value int
select @value = (select count(*) from dbo.typepassbook as t,dbo.passbook as p where p.passbook_type = t.id and  t.id = @id and  month(p.withdrawday) = @month and year(p.withdrawday) = @year and day(p.withdrawday) = @day)
return @value
end
go
--drop TABLE dbo.Calendar
CREATE TABLE dbo.Calendar (
    calendar_date    DATETIME    NOT NULL,
    CONSTRAINT PK_Calendar PRIMARY KEY CLUSTERED (calendar_date)
)
go
DECLARE @dIncr DATE = '2000-01-01'
DECLARE @dEnd DATE = '2100-01-01'

WHILE ( @dIncr < @dEnd )
BEGIN
  INSERT INTO Calendar (calendar_date) VALUES( @dIncr )
  SELECT @dIncr = DATEADD(DAY, 1, @dIncr )
END
--drop proc usp_ReportTypePassbookMonth
create proc usp_ReportTypePassbookMonth
@month int,
@year int,
@typeid int
as
begin
declare @startday int
declare @lastday int
select @startday = 1
select @lastday = day(DATEADD(month, ((@year - 1900) * 12) + 5, -1))
declare @startdate datetime
select @startdate = datefromparts(@year, @month, @startday)
declare @lastdate datetime
select @lastdate = datefromparts(@year, @month, @lastday)

select ROW_NUMBER() over(order by calendar_date) STT , calendar_date Day ,
dbo.reportTypeOpen(@typeid, day(calendar_date),month(calendar_date),year(calendar_date)) openP,
dbo.reportTypeClose(@typeid, day(calendar_date),month(calendar_date),year(calendar_date)) closeP,
abs(dbo.reportTypeOpen(@typeid, day(calendar_date),month(calendar_date),year(calendar_date)) - dbo.reportTypeClose(@typeid, day(calendar_date),month(calendar_date),year(calendar_date))) Difference
from dbo.Calendar 
where calendar_date between @startdate and @lastdate
--left join openP PassOpen , closeP PassClose, abs(openP - closeP) Difference
--from (select id as idc,
--case when openP is null then 0 else openP end as openP,
--case when closeP is null then 0 else closeP end as closeP
--from 
-- (select id, dbo.reportTypeOpen(id,@month,@year) as openP , dbo.reportTypeClose(id,@month,@year) as closeP
-- from typepassbook)as a) b,typepassbook where idc=typepassbook.id and typepassbook.id=@typeid
end
go
	--Tạo phiếu rút có ngày rút
	create proc usp_Insertwithdrawbill
	@id int,
	@passbook int,
	@money money,
	@day datetime
	as 
	begin
	insert dbo.withdrawbill(id,withdraw_passbook,withdrawmoney,withdrawdate)
	values
	(@id,
	@passbook,
	@money,
	@day)
	end
	go
	--Tạp phiếu rút không có ngày rút
	create proc usp_Insertwithdrawbill1
	@id int,
	@passbook int,
	@money money
	as
	insert dbo.withdrawbill(id,withdraw_passbook,withdrawmoney)
	values
	(@id,
	@passbook,
	@money)
	go
	--Xóa loại tiết kiệm 
	create proc usp_DeleteTypePassbook 
	@id int
	as
	begin
	declare @count int
	select @count=(select count(*) from passbook where passbook_type=@id and status=1)
	if(@count!=0)
	begin
	print N'Vẫn còn sổ sử dụng loại tiết kiệm này'
	rollback
	end
	else
	begin 
	print N'Xóa loại tiết kiệm thành công'
	delete withdrawbill where withdraw_passbook in (select id from passbook where passbook_type=@id)
	delete collectbill where collect_passbook in (select id from passbook where passbook_type=@id)
	delete passbook where passbook_type=@id
	delete typepassbook where id=@id
	end
	end

	go
	-- DU LIEU GIA
	--typepassbook
	exec usp_InsertTypePassbook 0.005,3,1000000,100000
	exec usp_InsertTypePassbook 0.0055,6,1000000,100000
	exec usp_InsertTypePassbook 0.0015,0,1000000,100000
	
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
	
	-- passbook
	exec usp_InsertPassbook 1,0,1,'20190215'
	exec usp_InsertPassbook 2,0,2,'20170601'
	exec usp_InsertPassbook 3,0,3,'20190722'
	exec usp_InsertPassbook 1,0,4,'20180813'
	exec usp_InsertPassbook 2,0,5,'20180912'
	exec usp_InsertPassbook 3,0,6,'20190405'
	exec usp_InsertPassbook 1,0,7,'20191104'
	exec usp_InsertPassbook 2,0,8,'20191228'
	exec usp_InsertPassbook 3,0,9,'20190120'
	exec usp_InsertPassbook 1,0,10,'20180228'
	exec usp_InsertPassbook 2,0,11,'20190330'
	exec usp_InsertPassbook 3,0,12,'20180430'
	exec usp_InsertPassbook 1,0,13,'20170531'
	exec usp_InsertPassbook 2,0,14,'20180616'
	exec usp_InsertPassbook 3,0,15,'20180717'
	exec usp_InsertPassbook 1,0,16,'20170818'
	exec usp_InsertPassbook 2,0,17,'20180922'
	exec usp_InsertPassbook 3,0,18,'20191023'
	exec usp_InsertPassbook 1,0,19,'20190521'
	exec usp_InsertPassbook 1,0,20,'20170426'
	
	--collectbill
	-- bill tạo sổ
	exec usp_Insertcollectbill 1,1,1000000,'20190215'
	exec usp_Insertcollectbill 2,2,2000000,'20170601'
	exec usp_Insertcollectbill 3,3,3000000,'20190722'
	exec usp_Insertcollectbill 4,4,4000000,'20180813'
	exec usp_Insertcollectbill 5,5,5000000,'20180912'
	exec usp_Insertcollectbill 6,6,6000000,'20190405'
	exec usp_Insertcollectbill 7,7,7000000,'20191104'
	exec usp_Insertcollectbill 8,8,8000000,'20191228'
	exec usp_Insertcollectbill 9,9,9000000,'20190120'
	exec usp_Insertcollectbill 10,10,10000000,'20180228'
	exec usp_Insertcollectbill 11,11,1100000,'20190330'
	exec usp_Insertcollectbill 12,12,1200000,'20180430'
	exec usp_Insertcollectbill 13,13,1300000,'20170531'
	exec usp_Insertcollectbill 14,14,1400000,'20180616'
	exec usp_Insertcollectbill 15,15,1500000,'20180717'
	exec usp_Insertcollectbill 16,16,1600000,'20170818'
	exec usp_Insertcollectbill 17,17,1700000,'20180922'
	exec usp_Insertcollectbill 18,18,1800000,'20191023'
	exec usp_Insertcollectbill 19,19,1900000,'20190521'
	exec usp_Insertcollectbill 20,20,1000000,'20170426'
	--
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
	
	--withdrawbill
	exec usp_Insertwithdrawbill 1,12,200000,'20180515'
	exec usp_Insertwithdrawbill 2,13,1300000,'20170829'
	exec usp_Insertwithdrawbill 3,14,1400000,'20181213'
	exec usp_Insertwithdrawbill 4,15,500000,'20180801'
	exec usp_Insertwithdrawbill 5,16,1600000,'20171116'
	exec usp_Insertwithdrawbill 6,17,1700000,'20190321'
	exec usp_Insertwithdrawbill 7,18,800000,'20191107'
	exec usp_Insertwithdrawbill 8,19,1900000,'20190819'
	exec usp_Insertwithdrawbill 9,20,1000000,'20170725'
	select * from withdrawbill
	select * from dbo.passbook
	select * from dbo.typepassbook
	select * from customer
	select * from collectbill
	go
	
go
create proc usp_SearchTranByCustomerName 
@cusname nvarchar
as
begin
select ROW_NUMBER() over(order by cus_name) STT, cc.id as [idtran], cc.collectdate as [datetran], N'Gửi tiền' as typetran ,
 c.cus_name as [cusname], t.typename  from dbo.customer as c,dbo.collectbill as cc,dbo.typepassbook as t,dbo.passbook as p 
where
c.id = p.passbook_customer and cc.collect_passbook = p.id and p.passbook_type = t.id and c.cus_name like '%'+@cusname+'%'
UNION
select ROW_NUMBER() over(order by cus_name) STT, cc.id as [idtran], cc.withdrawdate as [datetran], N'Rút tiền' as typetran ,
 c.cus_name as [cusname], t.typename  from dbo.customer as c,dbo.withdrawbill as cc,dbo.typepassbook as t,dbo.passbook as p 
where
c.id = p.passbook_customer and cc.withdraw_passbook = p.id and p.passbook_type = t.id and c.cus_name like '%'+@cusname+'%'
end

go 
create proc usp_SearchTranByCustomerNameAndDate
@cusname nvarchar,
@date datetime
as
begin
select ROW_NUMBER() over(order by cus_name) STT, cc.id as [idtran], cc.collectdate as [datetran], N'Gửi tiền' as typetran ,
 c.cus_name as [cusname], t.typename  from dbo.customer as c,dbo.collectbill as cc,dbo.typepassbook as t,dbo.passbook as p 
where
c.id = p.passbook_customer and cc.collect_passbook = p.id and p.passbook_type = t.id 
and c.cus_name like '%'+@cusname+'%'
and day(cc.collectdate) = day(@date) and month(cc.collectdate) = month(@date) and year(cc.collectdate) = year(@date)
UNION
select ROW_NUMBER() over(order by cus_name) STT, cc.id as [idtran], cc.withdrawdate as [datetran], N'Rút tiền' as typetran ,
 c.cus_name as [cusname], t.typename  from dbo.customer as c,dbo.withdrawbill as cc,dbo.typepassbook as t,dbo.passbook as p 
where
c.id = p.passbook_customer and cc.withdraw_passbook = p.id and p.passbook_type = t.id
and c.cus_name like '%'+@cusname+'%'
and day(cc.withdrawdate) = day(@date) and month(cc.withdrawdate) = month(@date) and year(cc.withdrawdate) = year(@date)
end

select * from passbook