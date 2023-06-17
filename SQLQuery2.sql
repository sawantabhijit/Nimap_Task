
use NIMAP_TASK;

CREATE TABLE Category (
    CategoryId INT PRIMARY KEY identity(1,1),
    CategoryName VARCHAR(255)
);

CREATE TABLE Product (
    ProductId INT PRIMARY KEY,
    ProductName VARCHAR(255),
    CategoryId INT,
    FOREIGN KEY (CategoryId) REFERENCES Category(CategoryId)
);


 select * from Category;
 insert into Category values (1,'mobile');
 insert into Category values (2,'laptop');
