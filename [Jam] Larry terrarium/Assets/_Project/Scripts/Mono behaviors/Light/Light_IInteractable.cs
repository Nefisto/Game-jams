public partial class Light
{
    public void Interact()
    {
        if (EnergySwitch)
            TurnSwitchOff();
        else
            TurnSwitchOn();
    }
}