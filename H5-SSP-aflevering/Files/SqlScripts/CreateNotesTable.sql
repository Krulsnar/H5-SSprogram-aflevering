use H5_SSP

create table Notes(
Id int identity(1,1) Primary Key not null,
AspNetUsersId nvarchar(450) foreign key references AspNetUsers(id) not null,
Notes nvarchar(1000) not null
);