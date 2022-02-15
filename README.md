# Production (in developing)
###### Разработано Копац Алексеем, гр. 3530202/90201
---
## Описание проекта
Данный проект представляет из себя .NET приложение для Windows, 
которое позволяет автоматизировать бизнес-процессы некой компании по производству и продаже Аудио и Видеотехники. 
Приложение рассчитано на 4 категории лиц пользователей: администратора системы, менеджеров, отвечающих за поставки товаров, продавцов и покупателей.

## Стек технологий и архитектура проекта
Основной код проекта написан на языке __C#__, а для визуализации интерфейса приложения использовалась система __WPF__. 
Для реализации базы данных приложения использовались СУБД Microsoft SQL Server 2019(!) и T-SQL. Бэкап базы данных в репозитории.

### Схема базы данных
[![BD-Schema.png](https://i.postimg.cc/9FgZ02L9/BD-Schema.png)](https://postimg.cc/0bmbXTR5)
---
В качестве архитектуры приложения была выбрана MVVM.

### Архитектурная диаграмма (MVVM)
[![Project-drawio.png](https://i.postimg.cc/KjBs7yNc/Project-drawio.png)](https://postimg.cc/dZQBvppg)

## Тестирование
Тестирование этого проекта было проведено методом "серого ящика", так как тестированием занимался сам разработчик.

---

## Скриншоты реализации

[![Enter.png](https://i.postimg.cc/26K71XNk/Enter.png)](https://postimg.cc/hzVdwsG6)

[![reg.png](https://i.postimg.cc/v84p3JBv/reg.png)](https://postimg.cc/MX8PGr1c)

[![AdmMenu.png](https://i.postimg.cc/MKkrtXgW/AdmMenu.png)](https://postimg.cc/s1ccgfV8)

[![productlist.png](https://i.postimg.cc/pdd0yj5B/productlist.png)](https://postimg.cc/Hrf0hV2J)

[![shops.png](https://i.postimg.cc/1tkMWYCW/shops.png)](https://postimg.cc/62hdqzNR)
