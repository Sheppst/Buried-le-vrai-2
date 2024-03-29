using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SM_Collisioner
{
    public enum CollProcessState
    {
        Active,
        Inactive,
        Damaged,
    }

    public enum CollCommand
    {
        Switch,
        Hit,
        ResumeAct,
        ResumeIn,
    }

    public class CollidProcess //Crée une nouvelle classe accessible en public
    {
        class StateTransition //Crée une classe StateTransition dans la classe Process, qui permettra d'avoir un tuple de deux variable enuméré
        {
            readonly CollProcessState CurrentState;//Définit une variable de la classe ne pouvant être modifié qu'à l'intérieur de cette dernière mais restant visible à l'éxterieur
            readonly CollCommand Command;//Définit une variable de la classe ne pouvant être modifié qu'à l'intérieur de cette dernière mais restant visible à l'éxterieur

            public StateTransition(CollProcessState currentState, CollCommand command)
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

        Dictionary<StateTransition, CollProcessState> transitions; //Définit une variable de la classe Process, étant un dictionnaire comprenant les variables de la classe StateTransition et l'état du ProcessState
        public CollProcessState CurrentState { get; private set; } //JSP

        public CollidProcess() // Crée une fonction ayant le même nom que la classe où il se trouve
        {
            CurrentState = CollProcessState.Inactive; // CurrentState est redéfinis à Inactive, mais je sais pas pourquoi 
            transitions = new Dictionary<StateTransition, CollProcessState> // Recrée un dictionnaire où est rempli chacune des réactions chronologique état/commande
                                                                    // avec à la suite l'état qui s'enchaine, ce dictionnaire est rédéfinis à chaque fois car le "out" fait sortir la variable du dictionnaire
                                                                    // à chaque fois que la fonction est appelé dans la classe public Program appellant GetNext()  
            {
            { new StateTransition(CollProcessState.Inactive, CollCommand.Switch), CollProcessState.Active },
            { new StateTransition(CollProcessState.Inactive, CollCommand.Hit), CollProcessState.Damaged },
            { new StateTransition(CollProcessState.Active, CollCommand.Switch), CollProcessState.Inactive },
            { new StateTransition(CollProcessState.Active, CollCommand.Hit), CollProcessState.Damaged },
            { new StateTransition(CollProcessState.Damaged, CollCommand.Hit), CollProcessState.Damaged },
            { new StateTransition(CollProcessState.Damaged, CollCommand.ResumeAct), CollProcessState.Active },
            { new StateTransition(CollProcessState.Damaged, CollCommand.ResumeIn), CollProcessState.Inactive },

            };
        }

        public CollProcessState GetNext(CollCommand command)
        {
            StateTransition transition = new StateTransition(CurrentState, command); // Définit une variable de StateTransition avec l'état actuelle et la commande demander 
            CollProcessState nextState; //Définit une variable ne pouvant comprendre que l'un des paramêtre de l'énumération ProcessState 
            if (!transitions.TryGetValue(transition, out nextState)) //Si aucune relation entre la l'état et la commande ne se fait renvoie un message d'erreur 
                throw new Exception("Invalid transition: " + CurrentState + " -> " + command); // Message d'erreur
            return nextState; // Sinon renvoie l'état correspondant à la relation état/commande
        }
        public CollProcessState MoveNext(CollCommand command)
        {
            CurrentState = GetNext(command); // Appelle GetNext pour définir un nouvelle état en fonction d'une commande demandé
            return CurrentState; // Renvoie l'état obtenu
        }
    }
}
