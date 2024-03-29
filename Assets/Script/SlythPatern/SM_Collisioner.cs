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

    public class CollidProcess //Cr�e une nouvelle classe accessible en public
    {
        class StateTransition //Cr�e une classe StateTransition dans la classe Process, qui permettra d'avoir un tuple de deux variable enum�r�
        {
            readonly CollProcessState CurrentState;//D�finit une variable de la classe ne pouvant �tre modifi� qu'� l'int�rieur de cette derni�re mais restant visible � l'�xterieur
            readonly CollCommand Command;//D�finit une variable de la classe ne pouvant �tre modifi� qu'� l'int�rieur de cette derni�re mais restant visible � l'�xterieur

            public StateTransition(CollProcessState currentState, CollCommand command)
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

        Dictionary<StateTransition, CollProcessState> transitions; //D�finit une variable de la classe Process, �tant un dictionnaire comprenant les variables de la classe StateTransition et l'�tat du ProcessState
        public CollProcessState CurrentState { get; private set; } //JSP

        public CollidProcess() // Cr�e une fonction ayant le m�me nom que la classe o� il se trouve
        {
            CurrentState = CollProcessState.Inactive; // CurrentState est red�finis � Inactive, mais je sais pas pourquoi 
            transitions = new Dictionary<StateTransition, CollProcessState> // Recr�e un dictionnaire o� est rempli chacune des r�actions chronologique �tat/commande
                                                                    // avec � la suite l'�tat qui s'enchaine, ce dictionnaire est r�d�finis � chaque fois car le "out" fait sortir la variable du dictionnaire
                                                                    // � chaque fois que la fonction est appel� dans la classe public Program appellant GetNext()  
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
            StateTransition transition = new StateTransition(CurrentState, command); // D�finit une variable de StateTransition avec l'�tat actuelle et la commande demander 
            CollProcessState nextState; //D�finit une variable ne pouvant comprendre que l'un des param�tre de l'�num�ration ProcessState 
            if (!transitions.TryGetValue(transition, out nextState)) //Si aucune relation entre la l'�tat et la commande ne se fait renvoie un message d'erreur 
                throw new Exception("Invalid transition: " + CurrentState + " -> " + command); // Message d'erreur
            return nextState; // Sinon renvoie l'�tat correspondant � la relation �tat/commande
        }
        public CollProcessState MoveNext(CollCommand command)
        {
            CurrentState = GetNext(command); // Appelle GetNext pour d�finir un nouvelle �tat en fonction d'une commande demand�
            return CurrentState; // Renvoie l'�tat obtenu
        }
    }
}
