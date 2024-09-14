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

BEGIN TRANSACTION;

DROP INDEX "IX_Photos_DriverId1";

ALTER TABLE "Photos" ADD "CarId" TEXT NULL;

CREATE TABLE "Cars" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Cars" PRIMARY KEY,
    "BrandModel" TEXT NOT NULL,
    "Pts" TEXT NOT NULL,
    "Sts" TEXT NOT NULL,
    "Vin" TEXT NOT NULL,
    "Year" TEXT NOT NULL,
    "RegistrationDate" TEXT NOT NULL,
    "PlateNumber" TEXT NOT NULL,
    "Osago" TEXT NOT NULL,
    "Kasko" TEXT NOT NULL,
    "TechInspection" TEXT NOT NULL,
    "TaxiLicense" INTEGER NOT NULL,
    "PurchaseOrRent" TEXT NOT NULL,
    "Leasing" INTEGER NOT NULL
);

CREATE TABLE "CarRentals" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_CarRentals" PRIMARY KEY AUTOINCREMENT,
    "DriverId" TEXT NOT NULL,
    "CarId" TEXT NOT NULL,
    "DriverName" TEXT NOT NULL,
    "CarName" TEXT NOT NULL,
    "Balance" TEXT NOT NULL,
    "Rent" TEXT NOT NULL,
    CONSTRAINT "FK_CarRentals_Cars_CarId" FOREIGN KEY ("CarId") REFERENCES "Cars" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_CarRentals_Drivers_DriverId" FOREIGN KEY ("DriverId") REFERENCES "Drivers" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Photos_CarId" ON "Photos" ("CarId");

CREATE INDEX "IX_Photos_DriverId" ON "Photos" ("DriverId");

CREATE INDEX "IX_CarRentals_CarId" ON "CarRentals" ("CarId");

CREATE INDEX "IX_CarRentals_DriverId" ON "CarRentals" ("DriverId");

CREATE TABLE "ef_temp_Photos" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Photos" PRIMARY KEY AUTOINCREMENT,
    "CarId" TEXT NULL,
    "DriverId" TEXT NULL,
    "Path" TEXT NOT NULL,
    CONSTRAINT "FK_Photos_Cars_CarId" FOREIGN KEY ("CarId") REFERENCES "Cars" ("Id"),
    CONSTRAINT "FK_Photos_Drivers_DriverId" FOREIGN KEY ("DriverId") REFERENCES "Drivers" ("Id")
);

INSERT INTO "ef_temp_Photos" ("Id", "CarId", "DriverId", "Path")
SELECT "Id", "CarId", "DriverId", "Path"
FROM "Photos";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;

DROP TABLE "Photos";

ALTER TABLE "ef_temp_Photos" RENAME TO "Photos";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;

CREATE INDEX "IX_Photos_CarId" ON "Photos" ("CarId");

CREATE INDEX "IX_Photos_DriverId" ON "Photos" ("DriverId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240912152829_rental', '6.0.33');

COMMIT;

BEGIN TRANSACTION;

ALTER TABLE "Photos" RENAME COLUMN "Path" TO "FileName";

ALTER TABLE "Photos" ADD "FileId" TEXT NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240914091645_photo-edit', '6.0.33');

COMMIT;