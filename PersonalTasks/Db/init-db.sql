use TasksDatabase;

Create Table task (
    id INT PRIMARY KEY IDENTITY(1,1),
	title VARCHAR(20) NOT NULL,
	description VARCHAR(50),
	completed BIT NOT NULL DEFAULT 0,
	created_at DATE NOT NULL DEFAULT GETDATE(),
	updated_at DATE,
);



CREATE TABLE users(
	id int PRIMARY KEY IDENTITY(1,1),
	username VARCHAR(20) UNIQUE NOT NULL,
	password_hashed VARCHAR(60) NOT NULL
);