/** @jsx createElement */
import { createElement ,Raw} from "@bikeshaving/crank";
import { renderer } from "@bikeshaving/crank/dom";

function Greeting({ name = "World" }) {
  return <div>Hello {name}</div>;
}

const RandomQuestion = async() => {
  let response = await fetch("http://localhost:1000/random-question/markup?label=Answer this&placeholder=Please complete this")
  let text = await response.text()
  return <Raw value={text}></Raw>
}

const BasicForm = async () => {
 
  return (
    <form method="post" action="http://localhost:1000/submit1">
      <div>
        <label>Name :</label>
        <input type="text" name="name" id="name"/>
      </div>
      <div>
        <label>Email :</label>
        <input type="text" name="emali" id="email"/>
      </div>
      <RandomQuestion/>
      <input type="submit"/>
    </form>
  )
}


renderer.render(<BasicForm />, document.body);
