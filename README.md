# 📘 Facebook Clone Backend

A backend-only clone of Facebook built with **C#**, **ASP.NET Core**, **Entity Framework Core**, and **MySQL** — focused on learning backend engineering and testing real-world features like authentication, friend management, posting, and more.

---

##  🚀 Tech Stack

- **Backend**: ASP.NET Core Web API
- **ORM**: Entity Framework Core
- **Database**: MySQL
- **Authentication**: JWT (JSON Web Tokens)
- **Media Storage**: Cloudinary (for image uploads)

---

## 🎯 Project Goals

This project is a **backend-only clone of Facebook**, created purely for learning and testing whether I can build a useful and scalable social networking backend.

> No frontend yet — but fully ready for integration with React/Next.js or any frontend framework.

---

## 🔐 Features

### 👤 **User Authentication**
- Register new users
- Login with JWT auth
- Secure password hashing
- Get current logged-in user
- Get user by username
- Update profile (including image upload to Cloudinary)
- Delete user account

---

### 👥 **Friend System**
- Send friend requests
- View sent/received friend requests
- Accept or decline friend requests
- Get all friends
- Unfriend users

---

### 📝 **Posts**
- Create a post (with or without image)
- Get all posts
- Get post by ID
- Update post (text and/or image)
- Delete post

---

### ❤️ **Likes**
- Like or unlike a post
- Get all likes on a post

---

### 💬 **Comments**
- Add a comment to a post
- Get all comments on a post

> _Update/delete comments and nested comments coming soon!_

---

## 📦 Setup Instructions

1. **Clone the repo**

```bash
git clone https://github.com/<your-username>/facebook-clone-backend.git
cd facebook-clone-backend
```

2. **Set up your appsettings.json or use secrets manager**
```bash
"ConnectionStrings": {
  "MySQL": "Your MySQL connection string"
},
"Jwt": {
  "SecurityKey": "YourSecretKeyHere",
  "Issuer": "FacebookClone",
  "Audience": "FacebookClone",
  "ExpireTime": 7
}
```
🔒 Make sure to never commit secrets. Use .env or ASP.NET Secret Manager.

3. **Run the app**
```bash
dotnet ef database upate
dotnet run
```
---

## 🧠 Lessons Learned
Real-world JWT authentication and middleware

Structuring scalable Web APIs

Handling relational data (users, posts, likes, friends)

Media handling with Cloudinary

Git mistakes I will never repeat again 😅

---

## 📌 Disclaimer

This project is strictly for educational purposes. It's not affiliated with Facebook Inc.

---
## Created by @Rahull-Adk
