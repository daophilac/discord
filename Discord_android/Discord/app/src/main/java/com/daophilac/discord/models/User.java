package com.daophilac.discord.models;
enum Gender{
    Male, Female
}
public class User {
    private int UserID;
    private String Email;
    private String Password;
    private String UserName;
    private String FirstName;
    private String LastName;
    private Gender Gender;
    private String Image;

    public int getUserID() {
        return UserID;
    }

    public void setUserID(int userID) {
        UserID = userID;
    }

    public String getEmail() {
        return Email;
    }

    public void setEmail(String email) {
        Email = email;
    }

    public String getPassword() {
        return Password;
    }

    public void setPassword(String password) {
        Password = password;
    }

    public String getUserName() {
        return UserName;
    }

    public void setUserName(String userName) {
        UserName = userName;
    }

    public String getFirstName() {
        return FirstName;
    }

    public void setFirstName(String firstName) {
        FirstName = firstName;
    }

    public String getLastName() {
        return LastName;
    }

    public void setLastName(String lastName) {
        LastName = lastName;
    }

    public com.daophilac.discord.models.Gender getGender() {
        return Gender;
    }

    public void setGender(com.daophilac.discord.models.Gender gender) {
        Gender = gender;
    }

    public String getImage() {
        return Image;
    }

    public void setImage(String image) {
        Image = image;
    }
}
