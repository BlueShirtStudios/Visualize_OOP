<div align="center">

# 🧩 Visualize OOP

**See your object-oriented code the way it was designed.**

Turn your C# codebase into interactive UML-style diagrams and uncover the design patterns hiding inside.

![Status](https://img.shields.io/badge/Status-In%20development-orange)
![License](https://img.shields.io/badge/License-MIT-green)
![Powered by](https://img.shields.io/badge/Powered%20by-Analyse--OOP-blueviolet)

</div>

---

## 📌 Overview

Object orientation can feel like a black box at first, but using this tool within ***your codebase*** makes understanding it no problem.

Visualize OOP reads your source code and builds UML-like diagrams that display the relationships between your objects. Combined with a design pattern analyzer, it gives you a single place to understand how your objects behave and how your system fits together.

> 💡 **Built on [Analyse-OOP](https://github.com/BlueShirtStudios/Analyse-OOP).**
> The analysis engine started life inside this project. As the graphical side grew, I split the analysis into its own standalone library so each part could be developed, tested, and reused independently.

---

## ✨ Key Features

### 🌳 Class Hierarchy Generator
- Renders complex hierarchy trees across classes, interfaces, and abstract classes
- Highlights access modifiers and class members directly inside each diagram node

### 🎨 Design Pattern Detector
- Analyzes your codebase to determine which design patterns are in use
- Works from the global structure of the project down to smaller patterns between individual classes

### ⚡ Explore Seamlessly
- View your system as **UML** or **node diagrams** using the Microsoft Graph Library
- **Drag and drop** nodes to explore, rearrange, or edit the diagram
- **Print / export** the diagram as an image you can drop into docs or presentations

---

## 🔄 How It Works

```
 Your C# source ──► Analyse-OOP ──► Class & relationship data ──► Visualize OOP ──► Interactive diagram
   (Roslyn parsing)                                                (graph rendering)
```

1. Point the tool at your project folder.
2. **Analyse-OOP** parses the code and maps the relationships between classes.
3. **Visualize OOP** turns that data into a graph you can explore and customise.

---

## 💻 Getting Started

A packaged release is coming as soon as the first complete version is ready. For now, clone the repository:

```bash
git clone https://github.com/BlueShirtStudios/Visualize_OOP
```

---

## 🗺️ Roadmap

- [x] Upload files and build relationships between objects
- [x] Create graphical representations of objects
- [x] Build visual graph representations of objects, mapping relations, with toggle settings support *(in progress)*
- [ ] Colour-code the diagram based on relationships between objects
- [ ] Integrate design pattern tracking
- [ ] Architectural analysis of the codebase
- [ ] Custom themes the user can choose from

---

## 🤝 Contributing

Ideas, bug reports, and pull requests are welcome. For larger changes, please open an issue first so we can talk it through.

---

## 📄 License

Distributed under the MIT License. See `LICENSE` for details.
