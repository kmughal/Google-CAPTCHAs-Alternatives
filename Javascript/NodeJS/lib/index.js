const { applyHoneypotValidationOnFormFields, honeypotRoutes } = require("./honeypot")
const { initRandomQuestionRoutes, applyRandomQuestionValidation } = require("./random-question")
module.exports = { applyHoneypotValidationOnFormFields, honeypotRoutes, initRandomQuestionRoutes, applyRandomQuestionValidation }