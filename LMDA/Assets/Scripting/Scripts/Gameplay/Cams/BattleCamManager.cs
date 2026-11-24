using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class BattleCamManager : MonoBehaviour
{
    // Ça fonctionne pour faire changer le spline du dolly
    // juste à mettre le dolly en auto pis changer sa vitesse pour bouger la cam

    [SerializeField] CinemachineSplineDolly _dolly;
    PlayerInputs _inputs;

    private void Start()
    {
        _inputs = new();
        _inputs.Player.Enable();

        _inputs.Player.Interact.performed += ChangeSpline;
        ChangeSpline(new());
    }

    private void ChangeSpline(InputAction.CallbackContext obj)
    {
        Debug.Log("Changing Dolly");

        if (_dolly.Spline.Spline == _dolly.Spline.Splines[1])
            _dolly.Spline.Spline = _dolly.Spline.Splines[0];
        else
            _dolly.Spline.Spline = _dolly.Spline.Splines[1];
    }

}
