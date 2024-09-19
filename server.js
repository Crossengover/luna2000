const express = require("express");
const path = require("path");
const fs = require("fs");
const multer = require("multer");
const app = express();
const Docxtemplater = require("docxtemplater");
const PizZip = require("pizzip");

// Настройка парсера для JSON
app.use(express.json());
app.use(express.urlencoded({ extended: true }));
app.use(express.static(path.join(__dirname, "public")));

// Настройка multer для загрузки файлов
const storage = multer.diskStorage({
  destination: (req, file, cb) => {
    cb(null, path.join(__dirname, "public", "images")); // Папка для сохранения изображений
  },
  filename: (req, file, cb) => {
    cb(null, Date.now() + path.extname(file.originalname)); // Уникальное имя файла
  },
});
const upload = multer({ storage });

// Обработчик для сохранения автомобиля
app.post("/save-car", upload.array("photos", 10), (req, res) => {
  if (!req.files) {
    return res
      .status(400)
      .json({ success: false, message: "Файлы не загружены." });
  }

  const newCar = {
    id: req.body.id,
    brandModel: req.body.brandModel,
    pts: req.body.pts,
    sts: req.body.sts,
    vin: req.body.vin,
    year: req.body.year,
    registrationDate: req.body.registrationDate,
    plateNumber: req.body.plateNumber,
    osago: req.body.osago,
    kasko: req.body.kasko,
    techInspection: req.body.techInspection,
    taxiLicense: req.body.taxiLicense === "true",
    purchaseOrRent: req.body.purchaseOrRent,
    leasing: req.body.leasing === "true",
    photos: req.files.map((file) => file.filename),
  };

  fs.readFile(
    path.join(__dirname, "data", "cars.json"),
    "utf-8",
    (err, data) => {
      if (err) {
        console.error("Ошибка чтения файла:", err);
        return res
          .status(500)
          .json({ success: false, message: "Ошибка чтения файла." });
      }

      const cars = JSON.parse(data);
      cars.push(newCar);

      fs.writeFile(
        path.join(__dirname, "data", "cars.json"),
        JSON.stringify(cars, null, 2),
        (err) => {
          if (err) {
            console.error("Ошибка записи в файл:", err);
            return res
              .status(500)
              .json({ success: false, message: "Ошибка записи в файл." });
          }

          res.json({ success: true });
        }
      );
    }
  );
});
// Обработка POST-запроса для добавления водителя
app.post("/save-driver", upload.array("photos", 4), (req, res) => {
  // Проверка и получение данных из формы
  const newDriver = {
    id: req.body.id,
    fio: req.body.fio,
    passport: req.body.passport,
    registration: req.body.registration,
    address: req.body.address,
    driverLicense: req.body.driverLicense,
    parkingAddress: req.body.parkingAddress,
    contacts: req.body.contacts,
    note: req.body.note,
    photos: req.files.map((file) => file.filename), // Обработка загруженных файлов
  };

  // Чтение существующих данных из файла
  fs.readFile("data/drivers.json", "utf8", (err, data) => {
    if (err) {
      console.error("Ошибка чтения файла:", err);
      return res
        .status(500)
        .json({ success: false, message: "Ошибка сервера" });
    }

    let drivers = [];
    if (data) {
      drivers = JSON.parse(data);
    }

    // Добавление нового водителя
    drivers.push(newDriver);

    // Запись обновленных данных обратно в файл
    fs.writeFile(
      "data/drivers.json",
      JSON.stringify(drivers, null, 2),
      (err) => {
        if (err) {
          console.error("Ошибка записи файла:", err);
          return res
            .status(500)
            .json({ success: false, message: "Ошибка сервера" });
        }

        // Отправка успешного ответа клиенту
        res.json({ success: true });
      }
    );
  });
});

// Добавьте этот код в server.js для обработки запросов к файлу cars.json
app.get("/data/cars.json", (req, res) => {
  res.sendFile(path.join(__dirname, "data", "cars.json"));
});
app.use(express.static(path.join(__dirname, "public")));

// Обработка DELETE-запроса для удаления автомобиля
app.delete("/delete-car/:id", (req, res) => {
  const carId = req.params.id.toString(); // Преобразуем ID в строку
  console.log(`Получен запрос на удаление автомобиля с ID: ${carId}`);

  fs.readFile("data/cars.json", "utf8", (err, data) => {
    if (err) {
      console.error("Ошибка чтения файла:", err);
      return res
        .status(500)
        .json({ success: false, message: "Ошибка сервера" });
    }

    let cars = [];
    if (data) {
      cars = JSON.parse(data);
    }

    console.log(`Данные автомобилей: ${JSON.stringify(cars, null, 2)}`);

    // Фильтрация автомобилей по ID
    cars = cars.filter((car) => car.id.toString() !== carId); // Преобразуем ID в строку для сравнения
    const newLength = cars.length;

    fs.writeFile("data/cars.json", JSON.stringify(cars, null, 2), (err) => {
      if (err) {
        console.error("Ошибка записи файла:", err);
        return res
          .status(500)
          .json({ success: false, message: "Ошибка сервера" });
      }

      res.json({ success: true });
    });
  });
});

// Обработка POST-запроса для обновления автомобиля
app.post("/edit-car/:id", upload.array("photos", 10), (req, res) => {
  const carId = req.params.id.toString(); // Преобразуем ID в строку

  fs.readFile("data/cars.json", "utf8", (err, data) => {
    if (err) {
      console.error("Ошибка чтения файла:", err);
      return res
        .status(500)
        .json({ success: false, message: "Ошибка сервера" });
    }

    let cars = [];
    if (data) {
      cars = JSON.parse(data);
    }

    const carIndex = cars.findIndex((car) => car.id.toString() === carId);
    if (carIndex === -1) {
      return res
        .status(404)
        .json({ success: false, message: "Автомобиль не найден" });
    }

    // Обновляем информацию о автомобиле
    const updatedCar = {
      id: req.body.id,
      brandModel: req.body.brandModel,
      pts: req.body.pts,
      sts: req.body.sts,
      vin: req.body.vin,
      year: req.body.year,
      registrationDate: req.body.registrationDate,
      plateNumber: req.body.plateNumber,
      osago: req.body.osago,
      kasko: req.body.kasko,
      techInspection: req.body.techInspection,
      taxiLicense: req.body.taxiLicense === "true",
      purchaseOrRent: req.body.purchaseOrRent,
      leasing: req.body.leasing === "true",
      photos: req.files.map((file) => file.filename),
    };

    // Сохраняем старые фотографии, если новые не загружены
    if (!req.files.length) {
      updatedCar.photos = cars[carIndex].photos;
    }

    cars[carIndex] = updatedCar;

    fs.writeFile("data/cars.json", JSON.stringify(cars, null, 2), (err) => {
      if (err) {
        console.error("Ошибка записи файла:", err);
        return res
          .status(500)
          .json({ success: false, message: "Ошибка сервера" });
      }

      res.json({ success: true });
    });
  });
});

// Отдача данных водителей из файла
app.get("/data/drivers.json", (req, res) => {
  res.sendFile(path.join(__dirname, "data", "drivers.json"));
});

app.use(express.static(path.join(__dirname, "public")));

// Обработка DELETE-запроса для удаления водителя
app.delete("/delete-driver/:id", (req, res) => {
  const driverId = req.params.id;

  fs.readFile("data/drivers.json", "utf8", (err, data) => {
    if (err) {
      console.error("Ошибка чтения файла:", err);
      return res
        .status(500)
        .json({ success: false, message: "Ошибка сервера" });
    }

    let drivers = JSON.parse(data);
    const driverIndex = drivers.findIndex((d) => d.id === driverId);

    if (driverIndex !== -1) {
      // Удаляем фотографии водителя
      drivers[driverIndex].photos.forEach((photo) => {
        fs.unlink(path.join(__dirname, "public", "images", photo), (err) => {
          if (err) console.error("Ошибка удаления файла:", err);
        });
      });

      drivers.splice(driverIndex, 1);

      fs.writeFile(
        "data/drivers.json",
        JSON.stringify(drivers, null, 2),
        (err) => {
          if (err) {
            console.error("Ошибка записи файла:", err);
            return res
              .status(500)
              .json({ success: false, message: "Ошибка сервера" });
          }
          res.json({ success: true });
        }
      );
    } else {
      res.status(404).json({ success: false, message: "Водитель не найден" });
    }
  });
});

// Обработка POST-запроса для редактирования водителя
app.post("/edit-driver/:id", upload.array("photos", 4), (req, res) => {
  const driverId = req.params.id;

  fs.readFile("data/drivers.json", "utf8", (err, data) => {
    if (err) {
      console.error("Ошибка чтения файла:", err);
      return res
        .status(500)
        .json({ success: false, message: "Ошибка сервера" });
    }

    let drivers = [];
    if (data) {
      drivers = JSON.parse(data);
    }

    const driverIndex = drivers.findIndex((d) => d.id === driverId);
    if (driverIndex === -1) {
      return res
        .status(404)
        .json({ success: false, message: "Водитель не найден" });
    }

    const existingDriver = drivers[driverIndex];

    const updatedDriver = {
      ...existingDriver,
      fio: req.body.fio,
      passport: req.body.passport,
      registration: req.body.registration,
      address: req.body.address,
      driverLicense: req.body.driverLicense,
      parkingAddress: req.body.parkingAddress,
      contacts: req.body.contacts,
      note: req.body.note,
      photos: existingDriver.photos, // Сохраняем старые фотографии
    };

    // Обновление фотографий
    if (req.files.length > 0) {
      updatedDriver.photos = req.files.map((file) => file.filename); // Добавляем новые фотографии
    }

    drivers[driverIndex] = updatedDriver;

    fs.writeFile(
      "data/drivers.json",
      JSON.stringify(drivers, null, 2),
      (err) => {
        if (err) {
          console.error("Ошибка записи файла:", err);
          return res
            .status(500)
            .json({ success: false, message: "Ошибка сервера" });
        }

        res.json({ success: true });
      }
    );
  });
});

// Обработка POST-запроса для сохранения данных
app.post("/save-main-data", express.json(), (req, res) => {
  const { driver, car, balance, rent } = req.body;

  if (!driver || !car || !balance || !rent) {
    return res
      .status(400)
      .json({ success: false, message: "Все поля обязательны для заполнения" });
  }

  // Чтение существующих данных
  fs.readFile("data/main.json", "utf8", (err, data) => {
    if (err && err.code !== "ENOENT") {
      console.error("Ошибка чтения файла:", err);
      return res
        .status(500)
        .json({ success: false, message: "Ошибка сервера" });
    }

    let mainData = [];
    if (data) {
      mainData = JSON.parse(data);
    }

    // Добавляем новую запись
    mainData.push({ driver, car, balance, rent });

    // Запись обновленных данных в файл
    fs.writeFile("data/main.json", JSON.stringify(mainData, null, 2), (err) => {
      if (err) {
        console.error("Ошибка записи файла:", err);
        return res
          .status(500)
          .json({ success: false, message: "Ошибка сервера" });
      }
      res.json({ success: true });
    });
  });
});
// Отдача данных из main.json
app.get("/data/main.json", (req, res) => {
  res.sendFile(path.join(__dirname, "data", "main.json"));
});

// Обработка POST-запроса для сохранения данных из таблицы
app.post("/save-main-data", (req, res) => {
  const { driverId, carId, balance, rent } = req.body;

  fs.readFile("data/main.json", "utf8", (err, data) => {
    if (err) {
      console.error("Ошибка чтения файла:", err);
      return res
        .status(500)
        .json({ success: false, message: "Ошибка сервера" });
    }

    let mainData = [];
    if (data) {
      mainData = JSON.parse(data);
    }

    // Проверка на наличие записи с таким же driverId и carId
    const index = mainData.findIndex(
      (item) => item.driverId === driverId && item.carId === carId
    );
    if (index !== -1) {
      // Обновление существующей записи
      mainData[index] = { driverId, carId, balance, rent };
    } else {
      // Добавление новой записи
      mainData.push({ driverId, carId, balance, rent });
    }

    fs.writeFile("data/main.json", JSON.stringify(mainData, null, 2), (err) => {
      if (err) {
        console.error("Ошибка записи файла:", err);
        return res
          .status(500)
          .json({ success: false, message: "Ошибка сервера" });
      }

      res.json({ success: true });
    });
  });
});

// Обработка POST-запроса для сохранения данных в main.json
app.post("/save-main", express.json(), (req, res) => {
  const mainData = req.body;

  fs.writeFile("data/main.json", JSON.stringify(mainData, null, 2), (err) => {
    if (err) {
      console.error("Ошибка записи файла:", err);
      return res
        .status(500)
        .json({ success: false, message: "Ошибка сервера" });
    }
    res.json({ success: true });
  });
});
// Обработка POST-запроса для сохранения данных в main.json
app.post("/save-main", express.json(), (req, res) => {
  const mainData = req.body;

  fs.writeFile("data/main.json", JSON.stringify(mainData, null, 2), (err) => {
    if (err) {
      console.error("Ошибка записи файла:", err);
      return res
        .status(500)
        .json({ success: false, message: "Ошибка сервера" });
    }
    res.json({ success: true });
  });
});

// Настройка multer для загрузки файлов
const fileStorage = multer.diskStorage({
  destination: (req, file, cb) => {
    cb(null, path.join(__dirname, "public", "documents")); // Папка для сохранения файлов
  },
  filename: (req, file, cb) => {
    cb(null, Date.now() + path.extname(file.originalname)); // Уникальное имя файла
  },
});
const fileUpload = multer({ storage: fileStorage });

// Обработчик загрузки файла
app.post("/upload", fileUpload.single("file"), (req, res) => {
  if (!req.file) {
    return res.status(400).json({ message: "Файл не загружен." });
  }

  console.log("Загруженный файл:", req.file);

  res.json({ message: "Файл успешно загружен." });
});

// Получение списка файлов
app.get("/files", (req, res) => {
  const documentsPath = path.join(__dirname, "public", "documents");
  fs.readdir(documentsPath, (err, files) => {
    if (err) {
      console.error("Ошибка чтения папки:", err);
      return res
        .status(500)
        .json({ success: false, message: "Ошибка сервера" });
    }
    res.json(files);
  });
});

// Удаление файла
app.delete("/delete-file/:filename", (req, res) => {
  const fileName = req.params.filename;
  const filePath = path.join(__dirname, "public", "documents", fileName);

  fs.unlink(filePath, (err) => {
    if (err) {
      console.error("Ошибка удаления файла:", err);
      return res
        .status(500)
        .json({ success: false, message: "Ошибка сервера" });
    }
    res.json({ success: true });
  });
});

// Возвращаем список шаблонов
app.get("/templates", (req, res) => {
  const templatesDir = path.join(__dirname, "templates");
  fs.readdir(templatesDir, (err, files) => {
    if (err) {
      return res.status(500).send("Error reading templates directory");
    }
    res.json({ templates: files });
  });
});

// Возвращаем данные водителей
app.get("/drivers", (req, res) => {
  const drivers = JSON.parse(fs.readFileSync("./data/drivers.json"));
  res.json(drivers);
});

// Возвращаем данные машин
app.get("/cars", (req, res) => {
  const cars = JSON.parse(fs.readFileSync("./data/cars.json"));
  res.json(cars);
});

// Обработка генерации документа
app.post("/generate-document", async (req, res) => {
  const { template, driver, car } = req.body;

  // Получаем данные водителя и машины
  const drivers = JSON.parse(fs.readFileSync("./data/drivers.json"));
  const cars = JSON.parse(fs.readFileSync("./data/cars.json"));

  const selectedDriver = drivers.find((d) => d.id === driver);
  const selectedCar = cars.find((c) => c.id === car);

  if (!selectedDriver || !selectedCar) {
    return res.status(400).send("Driver or car not found");
  }

  // Читаем шаблон
  const templatePath = path.join(__dirname, "templates", template);
  const content = fs.readFileSync(templatePath, "binary");

  const zip = new PizZip(content);
  const doc = new Docxtemplater(zip);

  // Подставляем все данные
  doc.setData({
    ...selectedDriver, // Все поля водителя
    ...selectedCar, // Все поля машины
  });

  try {
    doc.render();
  } catch (error) {
    return res.status(500).send("Error rendering document");
  }

  const buf = doc.getZip().generate({ type: "nodebuffer" });

  // Устанавливаем заголовки для загрузки файла
  res.set(
    "Content-Disposition",
    `attachment; filename=document_${Date.now()}.docx`
  );
  res.set(
    "Content-Type",
    "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
  );

  // Отправляем сгенерированный документ клиенту
  res.send(buf);
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Сервер запущен на http://localhost:${PORT}`);
});
