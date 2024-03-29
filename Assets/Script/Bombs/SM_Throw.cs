using System;
using System.Collections.Generic;

namespace SM_Throw
{ 
    public enum ProcessState
    {
        Inactive,
        Active,
        Paused,
        Terminated
    }

    public enum Command
    {
        Begin,
        Pause,
        Exit
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
                { new StateTransition(ProcessState.Inactive, Command.Begin), ProcessState.Active },
                { new StateTransition(ProcessState.Active, Command.Pause), ProcessState.Paused },
                { new StateTransition(ProcessState.Paused, Command.Exit), ProcessState.Terminated }
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


    //public class Program // Exemple d'utilisation de la classe Process
    //{
    //    static void Main(string[] args)
    //    {
    //        Process p = new Process();
    //        Console.WriteLine("Current State = " + p.CurrentState);
    //        Console.WriteLine("Command.Begin: Current State = " + p.MoveNext(Command.Begin));
    //        Console.WriteLine("Command.Pause: Current State = " + p.MoveNext(Command.Pause));
    //        Console.WriteLine("Command.End: Current State = " + p.MoveNext(Command.End));
    //        Console.WriteLine("Command.Exit: Current State = " + p.MoveNext(Command.Exit));
    //        Console.ReadLine();
    //    }
    //}
}
