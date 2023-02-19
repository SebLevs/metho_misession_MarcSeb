using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CommandPromptManager : Manager<CommandPromptManager>
{
    public CommandInvoker playerCommandInvoker;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text doneCommands;
    public bool IsActive = false;
    private string _isValidCommand;
    private string _inputCommand;

    protected override void OnAwake()
    {
        playerCommandInvoker.Init();
    }

    protected override void OnStart()
    {
        base.OnStart();
        IsActive = false;
    }

    public void ToggleActivatePrompt(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (IsActive)
            {
                if (!SceneLoadManager.Instance.IsInTitleScreen && UIManager.Instance.CurrentView != UIManager.Instance.ViewOptionMenu)
                {
                    GameManager.Instance.ResumeGame();
                }
                DeActivate();
            }
            else
            {
                GameManager.Instance.PauseGame();
                Activate();
            }
        }
    }

    public void Activate()
    {
        Entity_Player.Instance.Controller.enabled = false;
        Entity_Player.Instance.enabled = false;
        inputField.gameObject.SetActive(true);
        doneCommands.gameObject.SetActive(true);
        IsActive = true;
        inputField.Select();
    }

    public void DeActivate()
    {
        Entity_Player.Instance.Controller.enabled = true;
        Entity_Player.Instance.enabled = true;
        inputField.gameObject.SetActive(false);
        doneCommands.gameObject.SetActive(false);
        IsActive = false;
    }

    public void CheckCommandPrompt()
    {
        _isValidCommand = "";
        _inputCommand = inputField.text.ToUpper();
        _inputCommand = _inputCommand.Replace(" ", "_");

        switch (_inputCommand)
        {
            case "GODMODE_ON":
            {
                playerCommandInvoker.DoCommand(playerCommandInvoker.command1.Value);
                break;
            }
            case "GODMODE_OFF":
            {
                playerCommandInvoker.UnDoCommand(playerCommandInvoker.command1.Value);
                break;
            }
            case "ATTACK_SPEED_UP":
            {
                playerCommandInvoker.DoCommand(playerCommandInvoker.command2.Value);
                break;
            }
            case "ATTACK_SPEED_DOWN":
            {
                playerCommandInvoker.UnDoCommand(playerCommandInvoker.command2.Value);
                break;
            }
            case "MOVE_SPEED_UP":
            {
                playerCommandInvoker.DoCommand(playerCommandInvoker.command3.Value);
                break;
            }
            case "MOVE_SPEED_DOWN":
            {
                playerCommandInvoker.UnDoCommand(playerCommandInvoker.command3.Value);
                break;
            }
            case "SPECIAL_ATTACK_SPEED_UP":
            {
                playerCommandInvoker.DoCommand(playerCommandInvoker.command4.Value);
                break;
            }
            case "SPECIAL_ATTACK_SPEED_DOWN":
            {
                playerCommandInvoker.UnDoCommand(playerCommandInvoker.command4.Value);
                break;
            }
            case "BOOM_DISTANCE_UP":
            {
                playerCommandInvoker.DoCommand(playerCommandInvoker.command5.Value);
                break;
            }
            case "BOOM_DISTANCE_DOWN":
            {
                playerCommandInvoker.UnDoCommand(playerCommandInvoker.command5.Value);
                break;
            }
            default:
            {
                _isValidCommand = " : Command Not Valid";
                break;
            }
        }
        if (doneCommands.textInfo.lineCount > 5)
        {
            doneCommands.text = "";
        }
        doneCommands.text = "\n" + _inputCommand + _isValidCommand + doneCommands.text;
        inputField.text = "";
        inputField.Select();
    }
    
}
