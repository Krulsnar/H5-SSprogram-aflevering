Create database H5_SSP_TODO
go

use H5_SSP_TODO
go

create table Notes(
Id int identity(1,1) Primary Key NOT NULL,
UserId nvarchar(MAX) NOT NULL,
Title nvarchar(MAX) NOT NULL,
Note nvarchar(MAX) NOT NULL
);