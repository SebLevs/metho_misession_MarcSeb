using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CommandPromptManager : Manager<CommandPromptManager>
{
    public CommandInvoker playerCommandInvoker;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text doneCommands;
    public bool isActive;
    private string _isValidCommand;
    private string _inputCommand;
    private readonly Dictionary<string, Action> _possibleCommands = new();

    protected override void OnStart()
    {
        base.OnStart();
        playerCommandInvoker.Init();
        isActive = false;
        Init();
    }

    public void ToggleActivatePrompt(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isActive)
            {
                if (!SceneLoadManager.Instance.IsInTitleScreen &&
                    UIManager.Instance.CurrentView != UIManager.Instance.ViewOptionMenu)
                {
                    GameManager.Instance.ResumeGame();
                }

                DeActivate();
            }
            else
            {
                Activate();
                GameManager.Instance.PauseGame();
            }
        }
    }

    private void Activate()
    {
        Entity_Player.Instance.Controller.enabled = false;
        Entity_Player.Instance.enabled = false;
        inputField.gameObject.SetActive(true);
        doneCommands.gameObject.SetActive(true);
        isActive = true;
        inputField.Select();
    }

    private void DeActivate()
    {
        Entity_Player.Instance.Controller.enabled = true;
        Entity_Player.Instance.enabled = true;
        inputField.gameObject.SetActive(false);
        doneCommands.gameObject.SetActive(false);
        isActive = false;
    }

    private void DoCommandInput(CommandType type)
    {
        playerCommandInvoker.DoCommand(playerCommandInvoker.CommandPromptDic[type]);
    }

    private void UnDoCommandInput(CommandType type)
    {
        playerCommandInvoker.UnDoCommand(playerCommandInvoker.CommandPromptDic[type]);
    }

    private void Init()
    {
        _possibleCommands.Add("GODMODE_ON", () => { DoCommandInput(CommandType.INVINCIBLE); });
        _possibleCommands.Add("GODMODE_OFF", () => { UnDoCommandInput(CommandType.INVINCIBLE); });

        _possibleCommands.Add("FULL_HEAL", () => { DoCommandInput(CommandType.FULL_HEAL); });
        _possibleCommands.Add("INSTA_DEATH", () => { UnDoCommandInput(CommandType.FULL_HEAL); });
        
        _possibleCommands.Add("TELEPORT_TO_PORTAL", () => { DoCommandInput(CommandType.TELEPORT_TO_BOSS); });
        _possibleCommands.Add("TELEPORT_TO_SPAWN", () => { UnDoCommandInput(CommandType.TELEPORT_TO_BOSS); });
        
        _possibleCommands.Add("MAX_GOLD", () => { DoCommandInput(CommandType.MAX_GOLD); });
        _possibleCommands.Add("MIN_GOLD", () => { UnDoCommandInput(CommandType.MAX_GOLD); });

        _possibleCommands.Add("ATTACK_SPEED_UP", () => { DoCommandInput(CommandType.ATTACK_SPEED); });
        _possibleCommands.Add("ATTACK_SPEED_DOWN", () => { UnDoCommandInput(CommandType.ATTACK_SPEED); });

        _possibleCommands.Add("BOOM_SPEED_UP", () => { DoCommandInput(CommandType.BOMMERANG_ATTACK_SPEED); });
        _possibleCommands.Add("BOOM_SPEED_DOWN", () => { UnDoCommandInput(CommandType.BOMMERANG_ATTACK_SPEED); });

        _possibleCommands.Add("BOOM_DISTANCE_UP", () => { DoCommandInput(CommandType.BOMMERANG_DISTANCE); });
        _possibleCommands.Add("BOOM_DISTANCE_DOWN", () => { UnDoCommandInput(CommandType.BOMMERANG_DISTANCE); });

        _possibleCommands.Add("MOVE_SPEED_UP", () => { DoCommandInput(CommandType.MOVE_SPEED); });
        _possibleCommands.Add("MOVE_SPEED_DOWN", () => { UnDoCommandInput(CommandType.MOVE_SPEED); });

        _possibleCommands.Add("HEALTH_UP", () => { DoCommandInput(CommandType.HEALTH_UP); });
        _possibleCommands.Add("HEALTH_DOWN", () => { UnDoCommandInput(CommandType.HEALTH_UP); });
        
        _possibleCommands.Add("DODGE_DELAY_DOWN", () => { DoCommandInput(CommandType.DODGE_DELAY_DOWN);});
        _possibleCommands.Add("DODGE_DELAY_UP", () => { UnDoCommandInput(CommandType.DODGE_DELAY_DOWN);});
    }

    public void CheckCommandPrompt()
    {
        _isValidCommand = "";
        _inputCommand = inputField.text.ToUpper();
        _inputCommand = _inputCommand.Replace(" ", "_");

        if (_possibleCommands.TryGetValue(_inputCommand, out Action actionToDo))
        {
            actionToDo.Invoke();
        }
        else
        {
            _isValidCommand = " : Command Not Valid";
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