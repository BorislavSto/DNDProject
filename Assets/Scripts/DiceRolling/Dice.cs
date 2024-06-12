using System;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [Serializable]
    private struct PlayerRollDice
    {
        [SerializeField] private DiceSides dice;
        [SerializeField] private int diceSides;
        [SerializeField] private bool roll;

        public DiceSides Dice => dice;
        public int DiceSideNumber => diceSides;
        public bool Rool => roll;
    }

    [SerializeField] private PlayerRollDice playerDice = default;
    [SerializeField] private DiceBody dice = default;
    private bool inZone;

    private void OnEnable()
    {
        InputEventHandler.OnInteractInput += Roll;
        InputEventHandler.OnPlayerInInteractZone += ctx => inZone = ctx;  //// TODO : add block from spamming dice rolling
    }

    private void OnDestroy()
    {
        InputEventHandler.OnInteractInput -= Roll;
        InputEventHandler.OnPlayerInInteractZone -= ctx => inZone = ctx;
    }

    public void Roll() // could add modifiers here but i think it should happen wherever this is called
    {
        if (!inZone)
            return;

        switch (playerDice.Dice)
        {
            case DiceSides.Four:
                if (dice)
                    dice.Roll(4, RandomRoll(4));
                break;
            case DiceSides.Six:
                dice.Roll(6, RandomRoll(6));
                break;
            case DiceSides.Twenty:
                dice.Roll(20, RandomRoll(20));
                break;
            default:
                break;
        }
    }

    private int RandomRoll(int sides)
    {
        return UnityEngine.Random.Range(1, sides + 1);
    }
}
