use dbaw;

create table registers(
id_user int PRIMARY KEY identity(1,1) not null,
login_user varchar(50) not null,
role_user varchar(100) not null,
password_user varchar(50) not null,
isBanned BIT NOT NULL DEFAULT 0
);
insert into registers (login_user, password_user, role_user, isBanned) values ('admin', 'admin', 'admin', 0)

CREATE TABLE premises(
    id_premises int PRIMARY KEY IDENTITY(1,1) not null,
    _type VARCHAR(100),
    _adress VARCHAR(50),
    _price VARCHAR(100),
    id_seller VARCHAR(20),
    seller_name VARCHAR(100),
    _time VARCHAR(365)
);

CREATE TABLE purchases (
    id_purchase int identity(1,1) not null,
    id_user int,
    id_premises int,
    purchase_date datetime,
    FOREIGN KEY (id_user) REFERENCES registers(id_user),
    FOREIGN KEY (id_premises) REFERENCES premises(id_premises)
);

INSERT INTO purchases (id_user, id_premises, purchase_date) VALUES (1, 1, '2024-02-24');

select * from premises;
select * from purchases;
select * from registers;

drop table registers;
drop table premises;
drop table purchases;

ALTER TABLE registers;