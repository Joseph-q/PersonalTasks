CREATE DATABASE TasksDatabase;

use TasksDatabase;

Create Table task (
    id INT PRIMARY KEY IDENTITY(1,1),
	title VARCHAR(20) NOT NULL,
	description VARCHAR(50),
	completed BIT NOT NULL DEFAULT 0,
	created_at DATE NOT NULL DEFAULT GETDATE(),
	updated_at DATE,
	user_id INT, -- Nueva columna que referencia al id de usuario
    FOREIGN KEY (user_id) REFERENCES users(id) -- Definir la FK que hace referencia a la tabla 'users'
);



CREATE TABLE users(
	id int PRIMARY KEY IDENTITY(1,1),
	username VARCHAR(20) UNIQUE NOT NULL,
	password_hashed VARCHAR(60) NOT NULL
);