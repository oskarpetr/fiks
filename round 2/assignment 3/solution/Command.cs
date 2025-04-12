namespace Uloha3;

public class Command
{
    public CommandType Type { get; set; }
    public int TankIndex { get; set; }
}

public enum CommandType
{
    ExclamationPoint,
    QuestionMark,
    NumberSign
}