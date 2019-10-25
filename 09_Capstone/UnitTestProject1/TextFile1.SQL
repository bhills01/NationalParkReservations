-- Remove all the data from  the database!
DELETE FROM reservation
DELETE FROM site
DELETE FROM campground
DELETE FROM park;

-- Insert some test data

-- WIPE IDENTITIES BACK TO 1
DBCC CHECKIDENT ('Park', RESEED, 0)
DBCC CHECKIDENT ('Campground', RESEED, 0)
DBCC CHECKIDENT ('Site', RESEED, 0)
DBCC CHECKIDENT ('Reservation', RESEED, 0)

-- Create 2 parks
insert park (
			[name]
			,[location]
			,[establish_date]
			,[area]
			,[visitors]
			,[description]) values ('kantana','ohio','1909-10-10',2463129,1257647, 'wetlands of the lake erie basin')

insert park (
			[name]
			,[location]
			,[establish_date]
			,[area]
			,[visitors]
			,[description]) values ('indanabat','ohio','1901-10-10',234249,17577, 'mountain of highland glaciers north of the lake erie basin')





-- Create 2 campgrounds

	INSERT INTO campground (
		[park_id]
		,[name]
		,[open_from_mm]
		,[open_to_mm]
		,[daily_fee]) VALUES (1,'Woodside Lake Park', '6', '10', 50.00);

		INSERT INTO campground (
		[park_id]
		,[name]
		,[open_from_mm]
		,[open_to_mm]
		,[daily_fee]) VALUES (2,'Kool Lakes RV Park', '6', '10', 30.00);

-- create 2 site id for (1 for each campgrounds)
insert site (
				[campground_id]
				,[site_number]
				,[max_occupancy]
				,[accessible]
				,[max_rv_length]
				,[utilities]) Values (1,1,6, 0, 20, 1)

insert site (
				[campground_id]
				,[site_number]
				,[max_occupancy]
				,[accessible]
				,[max_rv_length]
				,[utilities]) Values (2,1,6, 0, 20, 1)

--create 2 reservations

insert reservation (
					[site_id]
					,[name]
					,[from_date]
					,[to_date]
					,[create_date]) values(1, 'bob', '2019-10-10', '2019-10-20', '2019-10-24 09:40:22.113')
