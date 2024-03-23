using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Interfaces;
using PlayerRoles;

namespace VeryUsualDay
{
    public class Config : IConfig
    {
        [Description("Плагин включён? (bool)")]
        public bool IsEnabled { get; set; } = true;

        [Description("Debug включён? (bool)")]
        public bool Debug { get; set; } = false;

        [Description("Список вещей для СБ (Dictionary<string, List<ItemType>>)")]
        public Dictionary<string, List<ItemType>> SecurityItems { get; set; } = new Dictionary<string, List<ItemType>>
        {
            {
                "Стажёр",
                new List<ItemType>
                {
                    ItemType.GunCOM15,
                    ItemType.KeycardJanitor,
                    ItemType.Flashlight
                }
            },
            {
                "Рядовой",
                new List<ItemType>
                {
                    ItemType.KeycardGuard,
                    ItemType.ArmorLight,
                    ItemType.Radio,
                    ItemType.GrenadeFlash,
                    ItemType.Medkit,
                    ItemType.GunFSP9
                }
            },
            {
                "Сержант",
                new List<ItemType>
                {
                    ItemType.KeycardMTFPrivate,
                    ItemType.ArmorLight,
                    ItemType.Radio,
                    ItemType.GrenadeFlash,
                    ItemType.Medkit,
                    ItemType.GunCrossvec
                }
            },
            {
                "Лейтенант",
                new List<ItemType>
                {
                    ItemType.KeycardMTFOperative,
                    ItemType.ArmorCombat,
                    ItemType.Radio,
                    ItemType.GrenadeFlash,
                    ItemType.Medkit,
                    ItemType.GunE11SR,
                    ItemType.Adrenaline
                }
            },
            {
                "Глава",
                new List<ItemType>
                {
                    ItemType.GunE11SR,
                    ItemType.ArmorHeavy,
                    ItemType.GunShotgun,
                    ItemType.SCP500,
                    ItemType.Medkit,
                    ItemType.Adrenaline,
                    ItemType.Radio,
                    ItemType.KeycardMTFCaptain
                }
            }
        };
        [Description("Список патронов для СБ (выдаётся 60 шт.) (Dictionary<string, List<AmmoType>>)")]
        public Dictionary<string, List<AmmoType>> SecurityAmmo { get; set; } = new Dictionary<string, List<AmmoType>>
        {
            {
                "Стажёр",
                new List<AmmoType>
                {
                    AmmoType.Nato9
                }
            },
            {
                "Рядовой",
                new List<AmmoType>
                {
                    AmmoType.Nato9
                }
            },
            {
                "Сержант",
                new List<AmmoType>
                {
                    AmmoType.Nato9
                }
            },
            {
                "Лейтенант",
                new List<AmmoType>
                {
                    AmmoType.Nato556
                }
            },
            {
                "Глава",
                new List<AmmoType>
                {
                    AmmoType.Ammo12Gauge,
                    AmmoType.Nato556
                }
            }
        };
        [Description("Максимальное здоровье СБ (Dictionary<string, float>)")]
        public Dictionary<string, float> SecurityHealth { get; set; } = new Dictionary<string, float>
        {
            {
                "Стажёр",
                100f
            },
            {
                "Рядовой",
                100f
            },
            {
                "Сержант",
                100f
            },
            {
                "Лейтенант",
                170f
            },
            {
                "Глава",
                250f
            }
        };

        [Description("Список вещей для Научных Сотрудников (Dictionary<string, List<ItemType>>)")]
        public Dictionary<string, List<ItemType>> ScientificItems { get; set; } = new Dictionary<string, List<ItemType>>
        {
            {
                "Стажёр",
                new List<ItemType>
                {
                    ItemType.KeycardJanitor,
                    ItemType.Flashlight
                }
            },
            {
                "Исследователь",
                new List<ItemType>
                {
                    ItemType.KeycardScientist,
                    ItemType.KeycardZoneManager,
                    ItemType.Flashlight,
                    ItemType.Adrenaline,
                    ItemType.Radio
                }
            },
            {
                "Медик",
                new List<ItemType>
                {
                    ItemType.KeycardScientist,
                    ItemType.KeycardZoneManager,
                    ItemType.Painkillers,
                    ItemType.Adrenaline
                }
            },
            {
                "Инженер",
                new List<ItemType>
                {
                    ItemType.KeycardContainmentEngineer,
                    ItemType.Radio,
                    ItemType.Painkillers,
                    ItemType.Adrenaline,
                    ItemType.Medkit,
                    ItemType.Flashlight
                }
            },
            {
                "Психолог",
                new List<ItemType>
                {
                    ItemType.KeycardScientist,
                    ItemType.KeycardZoneManager,
                    ItemType.Adrenaline,
                    ItemType.Adrenaline,
                    ItemType.Painkillers
                }
            },
            {
                "Научный руководитель",
                new List<ItemType>
                {
                    ItemType.KeycardResearchCoordinator,
                    ItemType.Radio,
                    ItemType.Painkillers,
                    ItemType.Adrenaline,
                    ItemType.Medkit
                }
            },
            {
                "Глава",
                new List<ItemType>
                {
                    ItemType.KeycardFacilityManager,
                    ItemType.Radio,
                    ItemType.SCP500,
                    ItemType.GunCOM18,
                    ItemType.ArmorCombat,
                    ItemType.Medkit
                }
            }
        };
        [Description("Список вещей для Уборщиков (Dictionary<string, List<ItemType>>)")]
        public Dictionary<string, List<ItemType>> JanitorsItems { get; set; } = new Dictionary<string, List<ItemType>>
        {
            {
                "Уборщик",
                new List<ItemType>
                {
                    ItemType.KeycardJanitor,
                    ItemType.Flashlight,
                    ItemType.Painkillers,
                    ItemType.Medkit,
                    ItemType.Adrenaline
                }
            },
            {
                "Старший уборщик",
                new List<ItemType>
                {
                    ItemType.KeycardJanitor,
                    ItemType.KeycardZoneManager,
                    ItemType.Flashlight,
                    ItemType.Painkillers,
                    ItemType.Medkit,
                    ItemType.Adrenaline
                }
            }
        };
        [Description("Список ролей для ЭВС (Dictionary<string, RoleTypeId>)")]
        public Dictionary<string, RoleTypeId> EMFRoles { get; set; } = new Dictionary<string, RoleTypeId>
        {
            {
                "Боец",
                RoleTypeId.NtfPrivate
            },
            {
                "Джаггернаут",
                RoleTypeId.NtfPrivate
            },
            {
                "Лейтенант",
                RoleTypeId.NtfSergeant
            },
            {
                "Экзобоец",
                RoleTypeId.NtfSpecialist
            },
            {
                "Глава",
                RoleTypeId.NtfCaptain
            }
        };
        [Description("Список вещей для ЭВС (Dictionary<string, List<ItemType>>)")]
        public Dictionary<string, List<ItemType>> EMFItems { get; set; } = new Dictionary<string, List<ItemType>>
        {
            {
                "Боец",
                new List<ItemType>
                {
                    ItemType.ArmorCombat,
                    ItemType.GunE11SR,
                    ItemType.Medkit,
                    ItemType.KeycardMTFOperative,
                    ItemType.Radio,
                    ItemType.Adrenaline,
                    ItemType.Painkillers
                }
            },
            {
                "Джаггернаут",
                new List<ItemType>
                {
                    ItemType.ArmorHeavy,
                    ItemType.GunLogicer,
                    ItemType.KeycardMTFOperative,
                    ItemType.Medkit,
                    ItemType.Medkit,
                    ItemType.Radio,
                    ItemType.Adrenaline
                }
            },
            {
                "Лейтенант",
                new List<ItemType>
                {
                    ItemType.ArmorHeavy,
                    ItemType.GunCrossvec,
                    ItemType.GunE11SR,
                    ItemType.KeycardMTFOperative,
                    ItemType.Medkit,
                    ItemType.Radio,
                    ItemType.Adrenaline
                }
            },
            {
                "Экзобоец",
                new List<ItemType>
                {
                    ItemType.ArmorHeavy,
                    ItemType.GunCom45,
                    ItemType.KeycardMTFOperative,
                    ItemType.Adrenaline,
                    ItemType.SCP500,
                    ItemType.GunShotgun,
                    ItemType.GunCOM18
                }
            },
            {
                "Глава",
                new List<ItemType>
                {
                    ItemType.GunFRMG0,
                    ItemType.GunShotgun,
                    ItemType.SCP500,
                    ItemType.Medkit,
                    ItemType.Adrenaline,
                    ItemType.Radio,
                    ItemType.KeycardMTFCaptain,
                    ItemType.ArmorHeavy
                }
            }
        };
        [Description("Максимальное здоровье ЭВС (Dictionary<string, float>)")]
        public Dictionary<string, float> EMFHealth { get; set; } = new Dictionary<string, float>
        {
            {
                "Боец",
                450f
            },
            {
                "Джаггернаут",
                600f
            },
            {
                "Лейтенант",
                500f
            },
            {
                "Экзобоец",
                700f
            },
            {
                "Глава",
                750f
            }
        };
        [Description("Список патронов для ЭВС (выдаётся 60 шт.) (Dictionary<string, List<AmmoType>>)")]
        public Dictionary<string, List<AmmoType>> EMFAmmo { get; set; } = new Dictionary<string, List<AmmoType>>
        {
            {
                "Боец",
                new List<AmmoType>
                {
                    AmmoType.Nato556
                }
            },
            {
                "Джаггернаут",
                new List<AmmoType>
                {
                    AmmoType.Nato762,
                    AmmoType.Nato762
                }
            },
            {
                "Лейтенант",
                new List<AmmoType>
                {
                    AmmoType.Nato556,
                    AmmoType.Nato9
                }
            },
            {
                "Экзобоец",
                new List<AmmoType>
                {
                    AmmoType.Nato9,
                    AmmoType.Nato9,
                    AmmoType.Nato9,
                    AmmoType.Ammo12Gauge
                }
            },
            {
                "Глава",
                new List<AmmoType>
                {
                    AmmoType.Ammo12Gauge,
                    AmmoType.Nato556,
                    AmmoType.Ammo12Gauge,
                    AmmoType.Nato556
                }
            }
        };
        [Description("Список вещей для Агентов (Dictionary<string, List<ItemType>>)")]
        public Dictionary<string, List<ItemType>> AgencyItems { get; set; } = new Dictionary<string, List<ItemType>>
        {
            {
                "Младший агент",
                new List<ItemType>
                {
                    ItemType.ArmorCombat,
                    ItemType.GunFSP9,
                    ItemType.GunCOM18,
                    ItemType.Radio,
                    ItemType.KeycardMTFPrivate,
                    ItemType.Medkit,
                    ItemType.Adrenaline
                }
            },
            {
                "Старший агент",
                new List<ItemType>
                {
                    ItemType.ArmorHeavy,
                    ItemType.GunCrossvec,
                    ItemType.GunRevolver,
                    ItemType.Radio,
                    ItemType.Medkit,
                    ItemType.Painkillers,
                    ItemType.KeycardMTFOperative
                }
            }
        };
        [Description("Здоровье Агентов (float)")]
        public float AgencyHealth { get; set; } = 350f;

        [Description("Список патронов для Агентов (выдаётся 60 шт.) (Dictionary<string, List<AmmoType>>)")]
        public Dictionary<string, List<AmmoType>> AgencyAmmo { get; set; } = new Dictionary<string, List<AmmoType>>
        {
            {
                "Младший агент",
                new List<AmmoType>
                {
                    AmmoType.Nato9,
                    AmmoType.Nato9
                }
            },
            {
                "Старший агент",
                new List<AmmoType>
                {
                    AmmoType.Nato9,
                    AmmoType.Ammo44Cal
                }
            }
        };

        [Description("Список вещей, которые не могут поднимать Научные Сотрудники (List<ItemType>)")]
        public List<ItemType> ForbiddenForScientists { get; set; } = new List<ItemType>
        {
            ItemType.GunCrossvec,
            ItemType.GunFRMG0,
            ItemType.GunE11SR,
            ItemType.GunAK,
            ItemType.GunLogicer,
            ItemType.GunShotgun,
            ItemType.GunA7,
            ItemType.GunCom45,
            ItemType.MicroHID
        };

        [Description("Список вещей, которые не могут поднимать Уборщики (List<ItemType>)")]
        public List<ItemType> ForbiddenForJanitors { get; set; } = new List<ItemType>
        {
            ItemType.GunCrossvec,
            ItemType.GunFRMG0,
            ItemType.GunFSP9,
            ItemType.GunE11SR,
            ItemType.GunAK,
            ItemType.GunLogicer,
            ItemType.GunShotgun,
            ItemType.GunA7
        };

        [Description("Инвентарь бойца БУО (List<ItemType>)")]
        public List<ItemType> BUOInventory { get; set; } = new List<ItemType>
        {
            ItemType.KeycardMTFPrivate,
            ItemType.GunShotgun,
            ItemType.GunRevolver,
            ItemType.Medkit,
            ItemType.Painkillers,
            ItemType.ArmorCombat,
        };

        [Description("Список ролей, на которые не распростроняется инфекиция SCP-008 (List<RoleTypeId>)")]
        public List<RoleTypeId> DoNotPoisonRoles { get; set; } = new List<RoleTypeId>
        {
            RoleTypeId.ChaosConscript,
            RoleTypeId.ChaosRifleman,
            RoleTypeId.ChaosMarauder,
            RoleTypeId.ChaosRepressor,
            RoleTypeId.NtfPrivate,
            RoleTypeId.NtfSpecialist,
            RoleTypeId.NtfSergeant,
            RoleTypeId.NtfCaptain
        };

        public List<ItemType> Scp035ForbiddenItems { get; set; } = new List<ItemType>
        {
            ItemType.GunA7,
            ItemType.GunAK,
            ItemType.GunCom45,
            ItemType.GunCrossvec,
            ItemType.GunE11SR,
            ItemType.GunFRMG0,
            ItemType.GunFSP9,
            ItemType.GunLogicer,
            ItemType.Painkillers,
            ItemType.Medkit
        };
    }
}
