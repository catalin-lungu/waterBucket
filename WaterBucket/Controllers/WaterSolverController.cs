using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WaterBucket.Models;

namespace WaterBucket.Controllers
{
    public class WaterSolverController : ApiController
    {
        // GET api/WatterSolver
        //[ResponseType(typeof(State))]
        public IEnumerable<State> GetSolutions(int bucket1, int bucket2, int quantity, int nrSolutions)
        {
            State root = new State(bucket1, bucket2, quantity);
            int nrOfSolutionsWanted = nrSolutions;

            Queue<State> q = new Queue<State>();
            q.Enqueue(root);

            List<State> checkedStates = new List<State>();
            List<State> solutions = new List<State>();
            int found = 0;

            while (q.Count > 0)
            {
                State current = q.Dequeue();
                if (current.IsSolution)
                {
                    solutions.Add(current);
                    found++;
                    if (found == nrOfSolutionsWanted)
                        break;
                    continue; // on this path there is a solution continue with other paths
                }

                //mark this as checked
                checkedStates.Add(current);

                //add all possible operations
                State state1 = current.EmptyBucket3();
                if (!checkedStates.Contains(state1) && !q.Any(x => x.Equals(state1)))
                {
                    q.Enqueue(state1);
                }

                State state2 = current.EmptyBucket5();
                if (!checkedStates.Contains(state2) && !q.Any(x => x.Equals(state2)))
                {
                    q.Enqueue(state2);
                }

                State state3 = current.FillBucket3();
                if (!checkedStates.Contains(state3) && !q.Any(x => x.Equals(state3)))
                {
                    q.Enqueue(state3);
                }

                State state4 = current.FillBucket5();
                if (!checkedStates.Contains(state4) && !q.Any(x => x.Equals(state4)))
                {
                    q.Enqueue(state4);
                }

                State state5 = current.SwitchBucket3IntoBucket5();
                if (!checkedStates.Contains(state5) && !q.Any(x => x.Equals(state5)))
                {
                    q.Enqueue(state5);
                }

                State state6 = current.SwitchBucket5IntoBucket3();
                if (!checkedStates.Contains(state6) && !q.Any(x => x.Equals(state6)))
                {
                    q.Enqueue(state6);
                }
            }

            return solutions;
        }
    }
}
