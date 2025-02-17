<h1 align="center">Food4Student web app</h1>

<p align="center">An attemp of me recreating my android app but using .NET microservices backend architecture and Nextjs frontend</p>


## How to run it?

<details>
    <summary>Click here for info</summary>

- Yeah there's nothing in here dumbass

- I haven't done the project yet be patient
</details>

## Dev log (more like my own diary)

Since yapping about my progress in the commit changes kinda "unprofessional"

So now i'm revoke to use these markdown files to yap about my develop progress

I'm starting to fed up with the daily routine of waking up and watching udemy video

So i will deadass drop the godot game and do this project instead

This is the web app microservice version for our original android Food4Students app

### Log 8

Which fucker think it is a great idea in order to use a css library you need to fucking config the tailwind.config.ts

I hope you get hit by a truck. It took me fucking 3 hours just to know why my fucking button doesn't appear or have an outline

### Log 7

I have fixed all the bug of the sync between restaurant and search service and all it cost is my sanity.

The thing that fuck me up the most is that i have no idea how the fucking publishEndpoint work

Like how come the created restaurant work even though when you publish the restaurant it isn't got an id in it

But when i try to update the fucking menu the restaurant with nested element when all of them id is 0

So for some reason the menu updated doesn't work like the created restaurant the id isn't auto THERE

So there are 2 fixes like you think.
- One is that you assign auto id for every nested element
- Or you can await publishEndpoint later

#### Attempt at fix 2

- I found out it the hard way that the publishEndpoint NEVER EVER FUCKING RUN IF YOU PUT IT AFTER THE context.saveChangesAsync()

- Like i even run it as a seperate function or make a WHOLE FREAKING ENDPOINT TO RUN THE publishEndpoint on that and it still didn't run

- Like whatever i do i can't make it to run if i put it after the context.saveChangesAsync()

So let's tackle the other way which is to assign a random Guid for every nested element (newly created food category, food item, variation and variation option)

#### Attempt at fix 1

- Which is to just assign id for every nested element which then you publish to searchService and then context.SaveChangesAsync()

- Welp that would have work if DbUpdateConcurrencyException ISN'T A FUCKING THING

- IT KEEP TELLING ME THAT IT EXPECTED 1 ROWS TO CHANGES BUT 0 ROWS CHANGES

- LIKE BOI DIDN'T YOU SEE I MAKE CHANGES TO THE FUCKING FOOD CATEGORY AND ITS NESTED ELEMENT ?

- Like bruh i can't like i try every way try catch and shit and assign id and shit and it still doesn't work

- At this point i'm just losing both myself and my mind i have been trying to assign id and finding out about that fucking stupid error

- And finally i found out about it the solution. Is to fucking play Valorant

- Then it hit me. I have 2 requirement for this to work.

#### The 2 requirements

1. In order for the nested element of the restaurant which is foodCategory, foodItem, variation and variationOption to have an id the context.SaveChangesAsync() have to run first

2. In order for the await publishEndpoint to run it has to be put BEFORE the context.SaveChangesAsync()

#### Solution ?

Make another changes to the current entity which change absolutely dogshit nothing then run the await publishEndpoint then under it run another context.SaveChangeAsync()

If you somehow find a solution for it to work without workaround like this. Call me please i'm desperate i need help and sleep

### Log 6

There's was a teeny tiny bug about syncing between the RestaurantService and SearchService

Which then turn into DbUpdateConcurrencyException which then turn into the SearchService not sync

But basically in short it fucked me up for hours but hey it work now

### Log 5

For those asking where tf have i been these 3 days. Welp i just spend 21 hours beating pokemon legends arceus

The game is good and worth it btw. Also leave some note here to do add later

Confirm the 

### Log 4

This is a fucking lot to config and i mean a fucking lot (not this commit tho since it just 2 file changes)

### Log 3

I don't actually give af about the restaurant id tie to userid tho like just make them unique. Fuck the id stuff.

### Log 2

The fucking reason why i'm deleting this the fucking 4th times is because there's a fucking .git folder in frontend

Then github doesn't know wtf that is and just fucking ignore it. Fuck me. 4 times create github repo back to back

### Log 1

Fuck i forgot to add the gitignore first then it just fucking take in all of the Debug folder

That's why you see this repo fucking open and delete and open and delete thrice

Fuck me