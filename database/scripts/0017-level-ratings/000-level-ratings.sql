alter table dbo.Levels
drop column RepresentativeRating;

alter table dbo.Levels
add Rating int null;

alter table dbo.Levels
add ToRating int null;

go

update dbo.Levels
set Rating = 
    case
        when Code = 'A1L' then 800
        when Code = 'A1H' then 900
        when Code = 'A2L' then 1000
        when Code = 'A2H' then 1100
        when Code = 'B1L' then 1200
        when Code = 'B1H' then 1300
        when Code = 'B1+L' then 1400
        when Code = 'B1+H' then 1500
        when Code = 'B2L' then 1600
        when Code = 'B2H' then 1700
        when Code = 'B2+L' then 1800
        when Code = 'B2+H' then 1900
        when Code = 'C1L' then 2000
        when Code = 'C1H' then 2100
        when Code = 'C2L' then 2200
        when Code = 'C2H' then 2300
    end
go

update dbo.Levels
set FromRating = Rating - 50;

update dbo.Levels
set ToRating = Rating + 49;

go

alter table dbo.Levels
alter column Rating int not null;

alter table dbo.Levels
alter column ToRating int not null;