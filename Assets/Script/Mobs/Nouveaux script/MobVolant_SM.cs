using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobVolant_SM
{
    public enum ProcessState
    {
        Inactive,
        Moved,
        DetectSmth,
        NoDetect,
        AttackSmth,
        SuccessHit,
        Damaged,
        Terminated
    }
    
    public enum Command
    {
        Begin,
        Detect,
        Attack,
        Hit,
        End, //Cette commande c'est plus sortir de l'�tat o� elle se trouve car �chec
        Resume, // Cette commande c'est plus sortir de l'�tat car r�ussite
        Death
    }
    
    public class Process //Cr�e une nouvelle classe accessible en public
    {
        class StateTransition //Cr�e une classe StateTransition dans la classe Process, qui permettra d'avoir un tuple de deux variable enum�r�
        {
            readonly ProcessState CurrentState;//D�finit une variable de la classe ne pouvant �tre modifi� qu'� l'int�rieur de cette derni�re mais restant visible � l'�xterieur
            readonly Command Command;//D�finit une variable de la classe ne pouvant �tre modifi� qu'� l'int�rieur de cette derni�re mais restant visible � l'�xterieur
    
            public StateTransition(ProcessState currentState, Command command)
            {
                CurrentState = currentState; //Red�finis l'�tat de la classe par un �tat donn� en param�tre 
                Command = command; //Stocke la commande donn� en param�tre
            }
    
            public override int GetHashCode()//JSP
            {
                return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
            }
    
            public override bool Equals(object obj)//JSP
            {
                StateTransition other = obj as StateTransition;
                return other != null && this.CurrentState == other.CurrentState && this.Command == other.Command;
            }
        }
    
        Dictionary<StateTransition, ProcessState> transitions; //D�finit une variable de la classe Process, �tant un dictionnaire comprenant les variables de la classe StateTransition et l'�tat du ProcessState
        public ProcessState CurrentState { get; private set; } //JSP
    
        public Process() // Cr�e une fonction ayant le m�me nom que la classe o� il se trouve
        {
            CurrentState = ProcessState.Inactive; // CurrentState est red�finis � Inactive, mais je sais pas pourquoi 
            transitions = new Dictionary<StateTransition, ProcessState> // Recr�e un dictionnaire o� est rempli chacune des r�actions chronologique �tat/commande
                                                                    // avec � la suite l'�tat qui s'enchaine, ce dictionnaire est r�d�finis � chaque fois car le "out" fait sortir la variable du dictionnaire
                                                                    // � chaque fois que la fonction est appel� dans la classe public Program appellant GetNext()  
        {
            { new StateTransition(ProcessState.Inactive, Command.Death), ProcessState.Terminated },
            { new StateTransition(ProcessState.Inactive, Command.Begin), ProcessState.Moved },
            { new StateTransition(ProcessState.Moved, Command.Detect), ProcessState.DetectSmth },
            { new StateTransition(ProcessState.Moved, Command.Hit), ProcessState.Damaged },
            { new StateTransition(ProcessState.Moved, Command.Death), ProcessState.Terminated },
            { new StateTransition(ProcessState.DetectSmth, Command.Attack), ProcessState.AttackSmth },
            { new StateTransition(ProcessState.DetectSmth, Command.End), ProcessState.Moved },
            { new StateTransition(ProcessState.DetectSmth, Command.Resume), ProcessState.Moved },
            { new StateTransition(ProcessState.DetectSmth, Command.Hit), ProcessState.Damaged },
            { new StateTransition(ProcessState.DetectSmth, Command.Death), ProcessState.Terminated },
            { new StateTransition(ProcessState.AttackSmth, Command.Resume), ProcessState.SuccessHit },
            { new StateTransition(ProcessState.AttackSmth, Command.End), ProcessState.NoDetect },
            { new StateTransition(ProcessState.AttackSmth, Command.Hit), ProcessState.Damaged },
            { new StateTransition(ProcessState.AttackSmth, Command.Death), ProcessState.Terminated },
            { new StateTransition(ProcessState.NoDetect, Command.Resume), ProcessState.Moved },
            { new StateTransition(ProcessState.NoDetect, Command.Detect), ProcessState.AttackSmth },
            { new StateTransition(ProcessState.NoDetect, Command.Hit), ProcessState.Damaged },
            { new StateTransition(ProcessState.NoDetect, Command.Death), ProcessState.Terminated },
            { new StateTransition(ProcessState.Damaged, Command.Resume), ProcessState.NoDetect },
            { new StateTransition(ProcessState.Damaged, Command.End), ProcessState.AttackSmth },
            { new StateTransition(ProcessState.Damaged, Command.Hit), ProcessState.Damaged },
            { new StateTransition(ProcessState.Damaged, Command.Death), ProcessState.Terminated },
            { new StateTransition(ProcessState.Damaged, Command.Detect), ProcessState.AttackSmth },
            { new StateTransition(ProcessState.SuccessHit, Command.Resume), ProcessState.NoDetect },
            { new StateTransition(ProcessState.SuccessHit, Command.End), ProcessState.Moved },
            { new StateTransition(ProcessState.SuccessHit, Command.Hit), ProcessState.Damaged },
            { new StateTransition(ProcessState.SuccessHit, Command.Death), ProcessState.Terminated },


        };
        }
    
        public ProcessState GetNext(Command command)
        {
            StateTransition transition = new StateTransition(CurrentState, command); // D�finit une variable de StateTransition avec l'�tat actuelle et la commande demander 
            ProcessState nextState; //D�finit une variable ne pouvant comprendre que l'un des param�tre de l'�num�ration ProcessState 
            if (!transitions.TryGetValue(transition, out nextState)) //Si aucune relation entre la l'�tat et la commande ne se fait renvoie un message d'erreur 
                throw new Exception("Invalid transition: " + CurrentState + " -> " + command); // Message d'erreur
            return nextState; // Sinon renvoie l'�tat correspondant � la relation �tat/commande
        }
        public ProcessState MoveNext(Command command)
        {
            CurrentState = GetNext(command); // Appelle GetNext pour d�finir un nouvelle �tat en fonction d'une commande demand�
            return CurrentState; // Renvoie l'�tat obtenu
        }
    }
}
