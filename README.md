# Measurements-TEST
 Проекты:
1. DeviceWebAPI - Веб сервис, который отдает имерения давления с устройства в формате json
2. MeasurementsApp  - небольшое asp.net mvc веб-приложение, которое показывет графики измерений по собранным данным со всех устройств.
3. MeasurementsLibrary  - библиотека доменной модели БД
4. MeasurementsWinService - Win службу, который опрашивает все устройства и сохраняет показания измерений в БД.
5. SimpleGeneratorDeviceData - простое WinForm приложение который генерирует новые измерения для выбранных усройств

Порядок действий:
1. Нужно изменить connectionstring в MeasurementsLibrary для подключения к БД
2. Запустить DeviceWebAPI на local IIS
3. Установить и запустить MeasurementsWinService
4. Запустить MeasurementsApp для просмотра графика
5. Запустить SimpleGeneratorDeviceData для добаления новых данных (Иногда выдает 412 при соединении с FTP. Пока просто перезапускаю приложение)
