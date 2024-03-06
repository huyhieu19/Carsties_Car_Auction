"use client";

import Listings from "./auctions/Listings";

export default function Home() {
  console.log("Server component");
  return (
    <div>
      <Listings />
    </div>
  );
}
