create table users
(
    id          serial
        primary key,
    firstname   varchar(100) not null,
    lastname    varchar(100) not null,
    email       varchar(255) not null
        unique,
    phonenumber varchar(20)  not null,
    createdat   timestamp default now()
);

alter table users
    owner to postgres;

create table books
(
    id              serial
        primary key,
    title           varchar(255) not null,
    author          varchar(255) not null,
    coverimageurl   text,
    publicationyear integer      not null,
    description     text,
    status          varchar(20) default 'available'::character varying
        constraint books_status_check
            check ((status)::text = ANY
                   (ARRAY [('available'::character varying)::text, ('borrowed'::character varying)::text])),
    createdat       timestamp   default now(),
    pdffilepath     text
);

alter table books
    owner to postgres;

create table borrowedbooks
(
    id         serial
        primary key,
    bookid     integer not null
        references books
            on delete cascade,
    userid     integer not null
        references users
            on delete cascade,
    borrowedat timestamp default now(),
    returnedat timestamp
);

alter table borrowedbooks
    owner to postgres;

