
let _questions = null
function RandomQuestion(questions) {
    const returnError = () => {
        throw new Error(`
                        Static questions are not provided please provide questions.
                        Normal example can be :
                            [
                                { 
                                    id : "1",
                                    question : "1+1",
                                    answner : "2"
                                }
                            ]
                        `)
    }

    _questions = JSON.parse(questions)
    const _getRandomIndex = () => _questions.length ? Math.floor(Math.random() * Math.floor(_questions.length)) : returnError()
    const _getRandomQuestion = () => _questions[_getRandomIndex()]

    const _validateRandomQuestion = data => {
        if (!data['question-id'] || !data['random-question']) new Error("Validation failed")

        let [questionId, awnser] = [data['question-id'], data['random-question']]

        const f = _questions.filter(a => a.id === questionId)

        if (!f || f.length === 0) new Error("Validation failed")

        return f[0].answer === awnser
    }

    return {
        validateRandomQuestion: _validateRandomQuestion,
        getRandomQuestionAndAnswer: () => _getRandomQuestion(),
        getMarkup: ({ label, placeholder, cssClass }) => {
            const q = _getRandomQuestion()
            return `
                    <label for="random-question">${label} ${q.question}</label>
                    <input type="hidden" name="question-id" id="question-id" value="${q.id}"/>
                    <input type="text" placeholder="${placeholder}" class="${cssClass || 'form-control'}" name="random-question" id="random-question"/>
                    `
        }
    }

}


module.exports.RandomQuestion = RandomQuestion



const routes = require("express").Router()

const fs = require("fs")
const readJson = filePath => {
    const fullPath = require("path").resolve(__dirname, filePath)
    if (!fs.existsSync(fullPath)) new Error(`${filePath} not found`)
    const contents = fs.readFileSync(fullPath, { encoding: "utf-8" })
    return contents
}

let _instance = null
function applyRandomQuestionValidation(req, res, next) {
    console.log(req.body, req.query, "inside")
    if (!_instance) new Error("initRandomQuestionRoutes is not used so fail to apply form validation for random question ")

    try {
        const result = _instance.validateRandomQuestion({ ...req.body, ...req.query })
        if (result) next()
        else throw new Error()
    } catch (_) {
        res.status(500).send("form not valid")
    }
}


function initRandomQuestionRoutes({ questions, filePath, addQuestionEndpoint }) {
    questions = filePath ? readJson(filePath) : questions
    _instance = RandomQuestion(questions)
    const normaliseData = req => ({ ...req.body, ...req.query })
    routes.get("/random-question/markup", (req, res) => {
        const data = normaliseData(req)
        const markup = _instance.getMarkup(data)
        res.send(markup).status(200)
    })

    addQuestionEndpoint && (routes.get("/random-question", (req, res) => {
        const data = normaliseData(req)
        const markup = instance.getRandomQuestionAndAnswer(data)
        res.setHeader("content-type", "application/json")
        res.json(markup).status(200)
    })
    )

    return routes
}

module.exports.initRandomQuestionRoutes = initRandomQuestionRoutes
module.exports.applyRandomQuestionValidation = applyRandomQuestionValidation