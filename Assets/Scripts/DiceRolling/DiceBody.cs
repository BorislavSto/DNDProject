using DG.Tweening;
using UnityEngine;

public class DiceBody : MonoBehaviour
{
    [SerializeField] private float forceUp = 3f;
    [SerializeField] private float rollDuration = 2f;
    [SerializeField] private float rotationAmount = 720f; // How much to rotate (in degrees)
    [SerializeField] private ForceMode forceMode = ForceMode.Impulse;

    private Rigidbody rb;

    private bool isRolling = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Roll(int sides, int rollResult)
    {
        rb.AddForce(transform.up * forceUp, forceMode);      /////// TODO: freeze rb after rotation to stop it from maybe rolling too far after

        if (isRolling) return;

        isRolling = true;

        // Get the predetermined final rotation based on the dice type and roll result
        Vector3 predeterminedFinalRotation = GetFinalRotation(sides, rollResult);
        Debug.Log("predetiremend rotation degrees " + predeterminedFinalRotation + " sides " + sides + " roll result " + rollResult);
        // Sequence for the dice roll animation
        Sequence rollSequence = DOTween.Sequence();

        // Add random rotations to simulate rolling
        rollSequence.Append(transform.DORotate(new Vector3(rotationAmount, 0, 0), rollDuration / 4, RotateMode.FastBeyond360));
        rollSequence.Join(transform.DORotate(new Vector3(0, rotationAmount, 0), rollDuration / 4, RotateMode.FastBeyond360));
        rollSequence.Join(transform.DORotate(new Vector3(0, 0, rotationAmount), rollDuration / 4, RotateMode.FastBeyond360));

        // Random final rotation to determine face up
        rollSequence.Append(transform.DORotate(predeterminedFinalRotation, rollDuration / 4, RotateMode.FastBeyond360));

        // On complete, reset the isRolling flag
        rollSequence.OnComplete(() =>
        {
            isRolling = false;
            //Debug.Log("Final Result: " + rollResult);
        });

        rollSequence.Play();

        //rb.DORotate(transform.right, 2f, RotateMode.FastBeyond360); TODO : learn how to use tweening
    }

    private Vector3 GetFinalRotation(int sides, int rollResult)
    {
        switch (sides)
        {
            case 4:
                return GetD4Rotation(rollResult);
            case 6:
                return GetD6Rotation(rollResult);
            case 20:
                return GetD20Rotation(rollResult);
            default:
                return Vector3.zero;
        }
    }

    private Vector3 GetD4Rotation(int rollResult)
    {
        // Define rotations for each face of a 4-sided dice
        switch (rollResult)
        {
            case 1: return new Vector3(-60, 45, 0);
            case 2: return new Vector3(60, 45, 0);
            case 3: return new Vector3(0, 0, 180);
            case 4: return new Vector3(60, -135, 0);
            default: return Vector3.zero;
        }
    }

    private Vector3 GetD6Rotation(int rollResult)
    {
        // Define rotations for each face of a 6-sided dice
        switch (rollResult)
        {
            case 1: return new Vector3(0, 0, 0);
            case 2: return new Vector3(0, 0, 90);
            case 3: return new Vector3(270, 0, 0);
            case 4: return new Vector3(90, 0, 0);
            case 5: return new Vector3(0, 0, 270);
            case 6: return new Vector3(180, 0, 0);
            default: return Vector3.zero;
        }
    }

    private Vector3 GetD20Rotation(int rollResult)
    {
        // Define rotations for each face of a 20-sided dice
        // This is a simplified example; the actual rotations will depend on the specific dice model
        return new Vector3(0, rollResult * 18, 0); // This is a placeholder
    }

    //private void AlignToFace(int sides, int rollResult)
    //{
    //    Vector3 targetUp = Vector3.zero;
    //    switch (sides)
    //    {
    //        case 4:
    //            targetUp = GetD4UpVector(rollResult);
    //            break;
    //        case 6:
    //            targetUp = GetD6UpVector(rollResult);
    //            break;
    //        case 20:
    //            targetUp = GetD20UpVector(rollResult);
    //            break;
    //    }
    //    Quaternion targetRotation = Quaternion.LookRotation(transform.forward, targetUp);
    //    transform.rotation = targetRotation;
    //}
}
