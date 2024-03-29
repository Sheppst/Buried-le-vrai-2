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
                { new StateTransition(ProcessState.Inactive, Command.Begin), ProcessState.Active },
                { new StateTransition(ProcessState.Active, Command.Pause), ProcessState.Paused },
                { new StateTransition(ProcessState.Paused, Command.Exit), ProcessState.Terminated }
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
