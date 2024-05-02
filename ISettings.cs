using Flex;

namespace FlexPortManagerPoC
{



    public interface ISettingsMinutesStep
    {
        [StorageLink(eSetting.Min)]
        public int MinutesMinimum { get; set; }
        [StorageLink(eSetting.Max)]
        public int MinutesMaximum { get; set; }
        [StorageLink(eSetting.First)]
        public int MinuteInit { get; set; }
        [StorageLink(eSetting.Step)]
        public int MinutesStep { get; set; }

    }

    public interface ISettingsPrice
    {

        [StorageLink(eSetting.Price10)]
        int PriceMethod { get; }
        [StorageLink(eSetting.Price1)]
        int Price { get; set; }
        [StorageLink(eSetting.Price2)]
        int PriceMinutes { get; set; }
        [StorageLink(eSetting.PriceRound)]
        int PriceRound { get; set; }
    }


    public interface ISettingsIOMotorAndHeading
    {
        [StorageLink(eSetting.FlexRelayA)]
        public int RelayMotor { get; set; }

        [StorageLink(eSetting.FlexRelayB)]
        public int RelayHeating { get; set; }

        [StorageLink(eSetting.FlexMinutes)]
        public int CoolingMinutes { get; set; }

    }


}
