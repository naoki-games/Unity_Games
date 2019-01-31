namespace StateMachineSample{
	
	/// <summary>
	/// State
	/// </summary>
	public class State<T>{

		// Use this for initialization
		protected T owner;

		public State(T owner) {
			this.owner = owner;
		}
		
		// Update is called once per frame
		public virtual  void Enter () {
		
		}

		public virtual void Execute(){
		}

		public virtual void Exit(){
		}
	}
}
