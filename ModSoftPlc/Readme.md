Модуль исполнения программ, написанных на C# и эмулирующих работу ПЛК.

A module for executing programs written in C# and emulating PLC operation

Для работы программ создаются файлы в папке Server/Storage - пример SoftPlc_PRG_ID0.xml
Программа имеет индекс, согласно расположения в файле настроек.
Пока файл в папке Storage существует, загрузка происходит всегда из него.
Чтобы перезагрузить программу, нужно выполнить два одинаковых шага.
- открыть настройки модуля,  в требуемой программе изменить настройку Restart_Program в  true, сохранить и передать серверу
- После этого снова открыть настройки модуля и поменять переменную в false и снова передать данные серверу.
Это связано с тем, что сам модуль не умеет работать с комментариями в xml файлах при сериализации.


For programs to work, files are created in the Server/Storage folder – an example SoftPlc_PRG_ID0.xml
The program has an index, according to the location in the settings file.
As long as the file exists in the Storage folder, the download always takes place from it.
To restart the program, you need to perform two identical steps.

– open the module settings, change the Restart_Program setting to true in the required program, save and transfer to the server.

– after that, open the module settings again and change the variable to false and transfer the data to the server again.

This is due to the fact that the module itself cannot work with comments in xml files during serialization.

