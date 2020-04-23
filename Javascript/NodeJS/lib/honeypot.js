
const express = require("express")

const types = {
    text: Symbol("text"),
    radio: Symbol("radio"),
    checkbox: Symbol("checkbox"),
    password: Symbol("password"),
    textArea: Symbol("textarea")
}

const router = express.Router()

const routes = [
    { routename: "/honeypot/text", type: types.text },
    { routename: "/honeypot/password", type: types.password },
    { routename: "/honeypot/radio", type: types.radio },
    { routename: "/honeypot/checkbox", type: types.checkbox }
]


function getHandler(req, res, r) {
    const data = { ...req.body, ...req.query }
    let params = { type: r.type, ...data }
    var markup = customMarkups(params)
    res.send(markup).status(200)
}

routes.forEach(r => router.get(r.routename, (req, res) => getHandler(req, res, r)))

module.exports.honeypotRoutes = router

const bcrypt = require("bcrypt")
const SALT = bcrypt.genSaltSync(10)
const generateHash = text => bcrypt.hashSync(text, SALT)

function customMarkups({ name, label, placeholder, type, value }) {
    const hashedName = generateHash(name)
    switch (type) {
        case types.text:
            return `
            <label for=${name}>${label}</label>
            <input type="text" class="form-control" name="mask-${name}" placeholder"${placeholder}"/>
        
            <label for=${name}>${label}</label>
            <input type="text" class="form-control" name="${hashedName}" placeholder"${placeholder}"/>
            `
        case types.password:
            return `
            <label for=${name}>${label}</label>
            <input type="password" class="form-control" name="mask-${name}" placeholder"${placeholder}"/>
        
            <label for=${name}>${label}</label>
            <input type="password" class="form-control" name="${hashedName}" placeholder"${placeholder}"/>
            `
        case types.checkbox:
            return `
            <input type="checkbox" class="form-control" name="mask-${name}" placeholder"${placeholder}"/>
            <label for=${name}>${label}</label>

            <input type="checkbox" class="form-control" name="${hashedName}" placeholder"${placeholder}"/>
            <label for=${name}>${label}</label>
            `
        case types.radio:
            return `
                <input type="radio" class="form-control" name="mask-${name}" placeholder"${placeholder}" value="${value}"/>
                <label for=${name}>${label}</label>

                <input type="radio" class="form-control" name="${hashedName}" placeholder"${placeholder}" value="${value}"/>
                <label for=${name}>${label}</label>
                `
    }
}


module.exports.applyHoneypotValidationOnFormFields = validateFormData

function validateFormData(req, res, next) {
    var list = []
    for (let i in req.body) {
        let isMaskedField = String(i).startsWith("mask")
        let botDetected = false
        if (isMaskedField) botDetected = (String(req.body[i]) || "").replace(/ +/g, "").length > 0
        if (botDetected) return res.status(500).send("form submission failed")

        if (isMaskedField) {
            let fieldname = i.replace("mask-", "")
            let hash = generateHash(fieldname)
            if (req.body[hash]) list.push({ [fieldname]: req.body[hash] })
        }
    }

    req.body = list
    next()
}