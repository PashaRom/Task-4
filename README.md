# Task-4

   В проекте присутствуют два json файла:
    - testconfig.json - конфигурационный файл;
    - testdata.json - файл с тестовыми данными.
   Настройки testconfig.json:
    browsers:<browser name>:isActive - указывает активный браузер для тестирования;
    browsers:<browser name>:pathToDriver - путь к web драйверу;
    browsers:<browser name>:options:leaveBrowserRunning - робота браузера одновременно с уже запущенным экземпляром;
    browsers:<browser name>:options:startSizeWindow - размер экрана для тестирования;
    browsers:<browser name>:options:mode - режим браузера;
    browsers:<browser name>:options:implicitWait - время неявного ожидания в секундах;
    browsers:<browser name>:options:explicitWait - время явного ожидания в секудах;
    url - адрес тестируемого ресурса;
    pathDownloadFiles - путь к директории для загрузки файлов.
                
    