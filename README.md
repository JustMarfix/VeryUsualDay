# VeryUsualDay

Это - плагин для закрытого РП-ивента "Слишком обычный день", который был придуман и проводился на сервере SCP:SL AmSerg Events.

Автор ивента - Kennie (@kennie4482 / @KennieSerg).

В случае, если этот репозиторий - публичный, вы можете свободно использовать плагин, если хотите проводить этот ивент, однако вам стоит учесть, что помимо плагина вам потребуется поднять БД и веб-сервер, их структура описана в одном из разделов ниже.

## Суть ивента
Ивент заключается в полной отыгровке рабочего дня персонала одного из комплексов (Зоны-02) SCP Foundation. Ивент проводится в режиме FULL RP со свободой отыгрыша, администраторы по сути своей лишь контролируют рп-процесс, не вмешиваясь в него слишком сильно.

Если вы хотите узнать больше - напишите Автору ивента, либо вернитесь на эту страницу, когда полное описание ивента будет добавлено сюда.

## Возможности плагина
> **Главный рычаг**

Командой-переключателем является `vudmode`. Эта команда включает и выключает плагин, с выключенным плагином все его функции перестанут работать. При выключении плагина все его параметры (количество прибывших бойцов БУО, информация об утечке SCP-008) сбрасываются к параметрам по умолчанию.

> **Возможности по умолчанию (работает пассивно, активируется после команды `vudmode`)**

1. При заходе на сервер игрок появится в башне за класс "Обучение", ему показывается broadcast про ивент.
2. После смерти игрок не остаётся в спектаторах, а появляется в башне за класс "Обучение", ему показывается соответствующий broadcast.
3. Научные сотрудники не могут брать CrossVec, пулемёт МОГ, MTFE!11SR, AK, Lohicer, Дробовик, A7, COM45, MicroHID.
4. Рабочие не могут брать: CrossVec, пулемёт МОГ, MTFE!11SR, AK, Lohicer, Дробовик, A7, COM45, FSP9.
5. В Зелёный/Изумрудный код трупы очищаются при появлении.
6. Если ClassD поднимет оружие в Зелёный/Изумрудный код - объявляется Синий.
7. SCP-035 не может выкинуть свой револьвер.
8. SCP-035 не может брать: Medkit/Painkillers + оружие, кроме: револьвер/пистолеты/jailbird/дробовик
9. SCP-035 имеет автозаполнение патрон на револьвер (стрельба без перезарядки)
10. SCP-035 не получает эффекты от использования: SCP-207, SCP-207?, SCP-500
11. SCP-008-2 получает мут при спавне, размут при смерти.

> **Команды (админские, управление игрой)**

1. `patogen008` - активирует утечку SCP-008 и блокирует первую дверь в К.С. 106. Утечка значит, что все игроки, находящиеся в тяжёлой зоне на протяжение утечки будут получать бесконечный эффект Poisoned, а каждый умерший человек через пару секунд превратится в SCP-008-2 (моделька Scp0492, 2075hp, эффект Stained (замедление) + Poisoned). Повторное использование команды прекращает утечку.
2. `roledistr` - спавнит людей по двум башням (военные и не-военные классы) согласно их ролям в БД. Для получения ролей из БД делает GET-запрос на поднятый HTTP-сервер.
3. `code <CODE>` - ставит игровой код (состояние раунда). Принимает параметры `green`, `emerald`, `blue`, `orange`, `yellow`, `red`.
4. `lunch` - запускает обед, кидает CASSIE о начале. Через 5 минут кидается CASSIE об окончании обеда.
5. `redoors` - закрывает все двери, кидает CASSIE об этом.\
6. `allowspawn` - позволяет игрокам использовать `.classd`.
7. `lock173gate` - блокирует ворота 173 в ТЗС (на этаже 049).
8. `lock049gate` - блокирует ворота 049 в ТЗС (на этаже 049).
9. `checkcode` - выводит текущий код.
10. `gocm <ID> [ID] ...` - отправляет на поверхность комплекса выбранных игроков с cassie прибытия персонала
11. `vudclear` - очищает вещи на карте, кроме тех, что перечислены в конфиге. (MedKit / Micro H.I.D. / MTF-E11-SR / COM-15 / Radio / Все патроны / SCP-500 / SCP-1853 / SCP-244)
12. `vudsupply <тип поставки> <тип предмета (для scp)> <количество (для scp/food)>` - поставляет запасы для комплекса.
12.1. Типы поставок - `med`, `emf`, `scp`, `food`, `security`.
12.2. Типы предметов - `500`, `1853`, `207`.
13. `breach244` - открывает все вазы (SCP-244 A/B). НУС SCP-244.
14. `recontain244` - закрывает все вазы (SCP-244 A/B). ВОУС SCP-244.

> **Команды (админские, спавн стажёров, которых нет в БД)**

Всем заспавнившимся кидается broadcast с кратким описанием игрового класса. У всех перечисленных ниже классов инвентарь и hp изменяются в конфиге.

1. `vudclassd <ID> [ID] ...` - спавнит Испытуемого(ых), телепортирует в камеру и связывает.
2. `vudworker <ID> [ID] ...` - спавнит Рабочего(их).
3. `vudscience <ID> [ID] ...` - спавнит Научного Сотрудника(ов).
4. `vudguard <ID> [ID] ...` - спавнит Охранника(ов).
5. `spawnbuo <ID> [ID] ...` - спавнит бойцов БУО.

> **Команды (админские, спавн SCP)**

У всех перечисленных ниже классов характеристики изменяются в конфиге.

1. `spawn008-2 <ID>` - спавнит SCP-008-2.
2. `spawn035 <ID>` - спавнит SCP-035.
3. `spawn035-2 <ID>` - спавнит 035-2.
4. `spawn049 <ID>` - спавнит SCP-049.
5. `spawn076-2 <ID>` - спавнит SCP-076-2.
6. `spawn372 <ID>` - спавнит SCP-372.
7. `spawn966 <ID>` - спавнит SCP-966.
8. `spawn682 <ID>` - спавнит SCP-682-MT. Постфикс MT означает нашу модификацию объекта.
9. `spawn999 <ID>` - спавнит SCP-999.

> **Команды (пользовательские)**

1. `.recontain008` - позволяет перекрыть распространение SCP-008. Доступно к использованию только в К.С. SCP-008.
2. `.classd` - позволяет игрокам из Обучения появиться за Испытуемых. Спавнит людей раз в 5 минут (отсчёт идёт с момента включения плагина), не больше 3х за раз, всего заспавненных самостоятельно не более 5. Заспавненные через `vudclassd` не учитываются в цифре 5.
3. `.code` - показывает код и статус обеда

## База данных

С самого начала, одной из ключевых фишек СОДа была лёгкость при старте: плагин сам поддерживал нужный режим игры и сам спавнил игроков за нужные роли. Эти роли + другие метаданные о игроках должны храниться в Базе Данных.
Я использовал MariaDB в качестве СУБД. Ниже описана структура таблицы `metalrp`.

```sql
CREATE TABLE `metalrp` (
  `discord` bigint(20) NOT NULL,
  `steamid` text DEFAULT NULL,
  `custominfo` text DEFAULT NULL,
  `name` text DEFAULT NULL,
  `role` text DEFAULT NULL,
  `department` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
```
Возможные значения для `department` - `СБ`, `ЭВС`, `Агентство`, `НС`, `Рабочие`, `Испытуемые`, `Административный персонал`.

Возможные значения для `role`:
1. `СБ`: `Стажёр`, `Рядовой`, `Сержант`, `Лейтенант`, `Глава`.
2. `ЭВС`: `Боец`, `Джаггернаут`, `Экзобоец`, `Глава`.
3. `Агентство`: `Младший агент`, `Старший агент`.
4. `НС`: `Стажёр`, `Психолог`, `Инженер`, `Исследователь`, `Медик`, `Научный руководитель`, `Глава`.
5. `Рабочие`: `Рабочий`, `Старший рабочий`.
6. `Испытуемые`: `Испытуемый`.
7. `Административный персонал`: `Менеджер зоны`, `Директор зоны`.

Мы использовали дискорд-бота для того, чтобы удобно работать с базой данных (потому в БД и используется поле `discord`), но вы можете придумать своё решение.

Чтобы плагин корректно работал с БД, вам надо поднять HTTP-сервер с подключением к БД, который будет отвечать на GET-запросы по пути `/get_user/<steamid>` JSON-ом с полями из БД:
```
[steamid, custominfo, name, department, role]
```
