\connect postgres

CREATE TABLE Users (
    Id UUID PRIMARY KEY,
    Login VARCHAR(50) NOT NULL,
    Password VARCHAR(100) NOT NULL
);
INSERT INTO Users(Id,Login,Password) VALUES ('a81bc81b-dead-4e5d-abff-90865d1e13b1','admin','8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918');
