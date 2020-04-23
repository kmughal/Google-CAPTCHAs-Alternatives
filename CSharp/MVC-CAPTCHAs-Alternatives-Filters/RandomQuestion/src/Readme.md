# Introduction

This is a google captcha alternative.

## How to use This

- Add the reference for this project by visiting https://www.nuget.org/packages/RandomQuestionFilter/
- You can add this to an action

```
  public class HomeController : Controller
  {
       public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [RandomQuestion.Filters.RandomQuestionValidationFilter]
        public IActionResult IndexPost()
        {
            if (ModelState.IsValid) return RedirectToAction("IndexSuccess");
            return View("Index");
        }
  }
```
- It is using the TagHelper for this you have to add the following line to _ViewImports.cshtml

```

@addTagHelper *,RandomQuestion.Filter

```

- Then in the partial where you want to create random question just add the following markup

```
 <random-question 
   prefix-label="Give answer :"
   placeholder="you have to provide answer..."></random-question>
```

- Important: When you will run this for the first time you will get an error abour missing random-question.json file.
After this error message you will notice that a new folder data has been created for you with some question / answer sample.
When you refresh the page you will see that a new question answer input box has appear. You can update that json file the code will
automatically fetch random question from that json for you.

- Feel free to fork this or change this!
