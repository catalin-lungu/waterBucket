using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterBucket.Models
{
    /// <summary>
    /// this represents a node
    /// </summary>
    public class State : IEquatable<State>
    {
        private int _bucket3;
        private int _bucket5;
        private int _bucket3Max;
        private int _bucket5Max;
        private int _target;
        private string _operation;
        private State _parentState;

        public bool IsSolution => _target != 0 && (_target == _bucket3 || _target == _bucket5);
        public string Bucket1 => _bucket3.ToString();
        public string Bucket2 => _bucket5.ToString();
        public string Operation => _operation;
        public State Parent => _parentState;

        public State(int bucket1, int bucket2, int target)
        {
            this._bucket3Max = bucket1;
            this._bucket5Max = bucket2;
            this._target = target;
        }

        private State(State state) : this(state._bucket3Max, state._bucket5Max, state._target)
        {
            //keep track of the path
            this._parentState = state;
        }

        #region possbile actions
        public State FillBucket3()
        {
            var newState = new State(this);
            newState._bucket5 = this._bucket5;
            newState._bucket3 = newState._bucket3Max;
            newState._operation = "fill bucket1";
            return newState;
        }

        public State FillBucket5()
        {
            var newState = new State(this);
            newState._bucket3 = this._bucket3;
            newState._bucket5 = newState._bucket5Max;
            newState._operation = "fill bucket2";
            return newState;
        }

        public State EmptyBucket3()
        {
            var newState = new State(this);
            newState._bucket5 = this._bucket5;
            newState._bucket3 = 0;
            newState._operation = "empty bucket1";
            return newState;
        }

        public State EmptyBucket5()
        {
            var newState = new State(this);
            newState._bucket3 = this._bucket3;
            newState._bucket5 = 0;
            newState._operation = "empty bucket2";
            return newState;
        }

        public State SwitchBucket3IntoBucket5()
        {
            var newState = new State(this);
            newState._bucket5 = this._bucket5 + this._bucket3;
            if (newState._bucket5 > newState._bucket5Max)
            {
                //if there is a diference keep it in bucket 3
                newState._bucket3 = newState._bucket5 - newState._bucket5Max;

                newState._bucket5 = _bucket5Max;
            }
            else // no dif
            {
                newState._bucket3 = 0;
            }
            newState._operation = "move bucket1 into bucket2";
            return newState;
        }

        public State SwitchBucket5IntoBucket3()
        {
            var newState = new State(this);
            newState._bucket3 = this._bucket3 + this._bucket5;
            if (newState._bucket3 > newState._bucket3Max)
            {
                //keep the dif in bucket 5
                newState._bucket5 = newState._bucket3 - newState._bucket3Max;

                newState._bucket3 = newState._bucket3Max;
            }
            else // no dif
            {
                newState._bucket5 = 0;
            }

            newState._operation = "move bucket2 into bucket1";
            return newState;
        }

        #endregion

        public bool Equals(State other)
        {
            if (this._bucket3 == other._bucket3 && this._bucket5 == other._bucket5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //build a representation string recursively up to root
        public override string ToString()
        {
            if (_parentState != null)
            {
                return $"{_parentState.ToString()} | {_operation} -> Bucket1: {_bucket3} and Bucket2: {_bucket5}";
            }
            else
            {
                return $"Bucket1: {_bucket3} and Bucket2: {_bucket5}";
            }
        }
    }
}