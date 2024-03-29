using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SM_Phase01
{
    public enum ProcessState
    {
        Inactive,
        ChoisingPhase,
        Bited,
        Charge,
        OnRooffed,
        ThrowedPoison,
        ThrowedWeb,
        SuccessBite,
        SuccessCharged,
        SuccessPoisoned,
        SuccessStunned,
        Terminated
    }

    public enum Command
    {
        Begin,
        Bit,
        Up,
        CutPhase,
        Down,
        HitSomeone,
        Web,
        Poison,
        Stun,
        Poisoned,
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
            { new StateTransition(ProcessState.Inactive, Command.Begin), ProcessState.ChoisingPhase },
            { new StateTransition(ProcessState.ChoisingPhase, Command.Bit), ProcessState.Bited },
            { new StateTransition(ProcessState.ChoisingPhase, Command.Up), ProcessState.OnRooffed },
            { new StateTransition(ProcessState.ChoisingPhase, Command.CutPhase), ProcessState.Charge },
            { new StateTransition(ProcessState.ChoisingPhase, Command.Death), ProcessState.Terminated },
            { new StateTransition(ProcessState.Bited, Command.HitSomeone), ProcessState.SuccessBite },
            { new StateTransition(ProcessState.Bited, Command.End), ProcessState.Inactive },
            { new StateTransition(ProcessState.Bited, Command.CutPhase), ProcessState.Inactive },
            { new StateTransition(ProcessState.Bited, Command.Death), ProcessState.Terminated },
            { new StateTransition(ProcessState.Charge, Command.HitSomeone), ProcessState.SuccessCharged },
            { new StateTransition(ProcessState.Charge, Command.End), ProcessState.Inactive },
            { new StateTransition(ProcessState.Charge, Command.CutPhase), ProcessState.Inactive },
            { new StateTransition(ProcessState.Charge, Command.Death), ProcessState.Terminated },
            { new StateTransition(ProcessState.OnRooffed, Command.Web), ProcessState.ThrowedWeb },
            { new StateTransition(ProcessState.OnRooffed, Command.Death), ProcessState.Terminated },
            { new StateTransition(ProcessState.ThrowedPoison, Command.Poisoned), ProcessState.SuccessPoisoned },
            { new StateTransition(ProcessState.ThrowedPoison, Command.Down), ProcessState.Charge },
            { new StateTransition(ProcessState.ThrowedPoison, Command.Death), ProcessState.Terminated },
            { new StateTransition(ProcessState.ThrowedWeb, Command.Stun), ProcessState.SuccessStunned },
            { new StateTransition(ProcessState.ThrowedWeb, Command.End), ProcessState.ThrowedPoison },
            { new StateTransition(ProcessState.ThrowedWeb, Command.Death), ProcessState.Terminated },
            { new StateTransition(ProcessState.SuccessBite, Command.Resume), ProcessState.Inactive },
            { new StateTransition(ProcessState.SuccessBite, Command.Death), ProcessState.Terminated },
            { new StateTransition(ProcessState.SuccessCharged, Command.Resume), ProcessState.Inactive },
            { new StateTransition(ProcessState.SuccessCharged, Command.Death), ProcessState.Terminated },
            { new StateTransition(ProcessState.SuccessPoisoned, Command.Down), ProcessState.Charge },
            { new StateTransition(ProcessState.SuccessPoisoned, Command.Death), ProcessState.Terminated },
            { new StateTransition(ProcessState.SuccessStunned, Command.Poison), ProcessState.ThrowedPoison },
            { new StateTransition(ProcessState.SuccessStunned, Command.End), ProcessState.ThrowedPoison },
            { new StateTransition(ProcessState.SuccessStunned, Command.Death), ProcessState.Terminated },


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
