const express = require("express")
const bodyParser = require("body-parser")
const cors = require("cors")
const app = express()


const { honeypotRoutes, applyHoneypotValidationOnFormFields, initRandomQuestionRoutes,applyRandomQuestionValidation } = require("./lib")
const randomQuestionRoute = initRandomQuestionRoutes({filePath : "../data/random-question.json"})

app.use(cors())
app.use(express.static("public"))
app.use(bodyParser())
app.use("/", honeypotRoutes)
app.use("/",randomQuestionRoute)
app.post("/submit", applyHoneypotValidationOnFormFields, (req, res) => {
    console.log("successfully")
    res.status(200).send("submitted")
})
app.post("/submit1"  , applyRandomQuestionValidation,(req,res) => {
    res.status(200).send("verified")
})


app.listen(1000, () => console.log("connected"))