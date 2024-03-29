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
        End, //Cette commande c'est plus sortir de l'état où elle se trouve car échec
        Resume, // Cette commande c'est plus sortir de l'état car réussite
        Death
    }
    
    public class Process //Crée une nouvelle classe accessible en public
    {
        class StateTransition //Crée une classe StateTransition dans la classe Process, qui permettra d'avoir un tuple de deux variable enuméré
        {
            readonly ProcessState CurrentState;//Définit une variable de la classe ne pouvant être modifié qu'à l'intérieur de cette dernière mais restant visible à l'éxterieur
            readonly Command Command;//Définit une variable de la classe ne pouvant être modifié qu'à l'intérieur de cette dernière mais restant visible à l'éxterieur
    
            public StateTransition(ProcessState currentState, Command command)
            {
                CurrentState = currentState; //Redéfinis l'état de la classe par un état donné en paramêtre 
                Command = command; //Stocke la commande donné en paramêtre
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
    
        Dictionary<StateTransition, ProcessState> transitions; //Définit une variable de la classe Process, étant un dictionnaire comprenant les variables de la classe StateTransition et l'état du ProcessState
        public ProcessState CurrentState { get; private set; } //JSP
    
        public Process() // Crée une fonction ayant le même nom que la classe où il se trouve
        {
            CurrentState = ProcessState.Inactive; // CurrentState est redéfinis à Inactive, mais je sais pas pourquoi 
            transitions = new Dictionary<StateTransition, ProcessState> // Recrée un dictionnaire où est rempli chacune des réactions chronologique état/commande
                                                                    // avec à la suite l'état qui s'enchaine, ce dictionnaire est rédéfinis à chaque fois car le "out" fait sortir la variable du dictionnaire
                                                                    // à chaque fois que la fonction est appelé dans la classe public Program appellant GetNext()  
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
            StateTransition transition = new StateTransition(CurrentState, command); // Définit une variable de StateTransition avec l'état actuelle et la commande demander 
            ProcessState nextState; //Définit une variable ne pouvant comprendre que l'un des paramêtre de l'énumération ProcessState 
            if (!transitions.TryGetValue(transition, out nextState)) //Si aucune relation entre la l'état et la commande ne se fait renvoie un message d'erreur 
                throw new Exception("Invalid transition: " + CurrentState + " -> " + command); // Message d'erreur
            return nextState; // Sinon renvoie l'état correspondant à la relation état/commande
        }
        public ProcessState MoveNext(Command command)
        {
            CurrentState = GetNext(command); // Appelle GetNext pour définir un nouvelle état en fonction d'une commande demandé
            return CurrentState; // Renvoie l'état obtenu
        }
    }
}
