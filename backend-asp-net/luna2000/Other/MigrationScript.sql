CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;

CREATE TABLE "Drivers" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Drivers" PRIMARY KEY,
    "Address" TEXT NOT NULL,
    "Contacts" TEXT NOT NULL,
    "DriverLicense" TEXT NOT NULL,
    "Fio" TEXT NOT NULL,
    "Note" TEXT NOT NULL,
    "ParkingAddress" TEXT NOT NULL,
    "Passport" TEXT NOT NULL,
    "Registration" TEXT NOT NULL
);

CREATE TABLE "Photos" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Photos" PRIMARY KEY AUTOINCREMENT,
    "Path" TEXT NOT NULL,
    "DriverId" INTEGER NOT NULL,
    "DriverId1" TEXT NOT NULL,
    CONSTRAINT "FK_Photos_Drivers_DriverId1" FOREIGN KEY ("DriverId1") REFERENCES "Drivers" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Photos_DriverId1" ON "Photos" ("DriverId1");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240911195247_initCreate', '6.0.33');

COMMIT;