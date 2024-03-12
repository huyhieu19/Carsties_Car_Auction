import React from "react";
import { AiOutlineCar } from "react-icons/ai";
import Search from "./Search";
import Logo from "./Logo";
import LoginButton from "./LoginButton";
import { getCurrentUser } from "../actions/authActions";
import { getSession } from "next-auth/react";
import UserActions from "./UserActions";

export default async function Navbar() {
  const user = await getCurrentUser();
  return (
    <header
      className="sticky top-0 flex z-50 justify-between
      bg-white p-5 items-center text-gray-800 shadow-md"
    >
      <Logo />
      <Search />
      {user ? <UserActions user={user} /> : <LoginButton />}
    </header>
  );
}
